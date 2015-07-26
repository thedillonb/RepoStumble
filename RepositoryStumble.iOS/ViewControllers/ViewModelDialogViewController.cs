using System;
using ReactiveUI;
using System.Reactive.Linq;
using RepositoryStumble.Core.ViewModels;
using UIKit;

namespace RepositoryStumble.ViewControllers
{
    public abstract class ViewModelDialogViewController<TViewModel> : DialogViewController, IViewFor<TViewModel> where TViewModel : class, IBaseViewModel
    {
        private UIRefreshControl _refreshControl;
        private bool _loaded;

        protected ViewModelDialogViewController (bool unevenRows = false, UITableViewStyle style = UITableViewStyle.Grouped) 
            : base(unevenRows, style)
        {
            NavigationItem.BackBarButtonItem = new UIBarButtonItem() { Title = string.Empty };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var loadableViewModel = ViewModel as ILoadableViewModel;
            if (loadableViewModel != null)
            {
                _refreshControl = new UIRefreshControl();
                RefreshControl = _refreshControl;
                _refreshControl.ValueChanged += (s, e) => loadableViewModel.LoadCommand.ExecuteIfCan();
                loadableViewModel.LoadCommand.IsExecuting.Where(x => !x).Subscribe(x => _refreshControl.EndRefreshing());
            }
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);

            var activatable = ViewModel as ISupportsActivation;
            if (activatable != null)
                activatable.Activator.Activate();

            if (!_loaded)
            {
                _loaded = true;
                var loadableViewModel = ViewModel as ILoadableViewModel;
                if (loadableViewModel != null)
                    loadableViewModel.LoadCommand.ExecuteIfCan();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            var activatable = ViewModel as ISupportsActivation;
            if (activatable != null)
                activatable.Activator.Deactivate();
        }

        private TViewModel _viewModel;
        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TViewModel)value; }
        }
    }
}