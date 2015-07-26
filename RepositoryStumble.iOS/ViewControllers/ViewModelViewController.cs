using ReactiveUI;
using System;
using RepositoryStumble.Core.ViewModels;
using Foundation;

namespace RepositoryStumble.ViewControllers
{
    public abstract class ViewModelViewController<TViewModel> : ReactiveViewController, IViewFor<TViewModel> where TViewModel : class, IBaseViewModel
    {
        public TViewModel ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TViewModel)value; }
        }

        protected bool ManualLoad { get; set; }

        protected ViewModelViewController()
        {
        }

        protected ViewModelViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected ViewModelViewController(string nibNameOrNull, NSBundle nibBundleOrNull)
            : base(nibNameOrNull, nibBundleOrNull)
        {
        }

        private bool _isLoaded;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var activatable = ViewModel as ISupportsActivation;
            if (activatable != null)
                activatable.Activator.Activate();

            if (!ManualLoad && !_isLoaded)
            {
                var loadableViewModel = ViewModel as ILoadableViewModel;
                if (loadableViewModel != null)
                    loadableViewModel.LoadCommand.ExecuteIfCan();
                _isLoaded = true;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            var activatable = ViewModel as ISupportsActivation;
            if (activatable != null)
                activatable.Activator.Deactivate();
        }
    }
}
