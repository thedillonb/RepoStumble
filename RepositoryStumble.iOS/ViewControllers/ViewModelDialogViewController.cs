using System;
using MonoTouch.Dialog;
using ReactiveUI;
using Xamarin.Utilities.Core.Services;
using MonoTouch.UIKit;
using Xamarin.Utilities.Core.ViewModels;

namespace RepositoryStumble.ViewControllers
{
    public abstract class ViewModelDialogViewController<TViewModel> : DialogViewController where TViewModel : ReactiveObject
    {
        private readonly TViewModel _viewModel = IoC.Resolve<TViewModel>();
        protected readonly INetworkActivityService NetworkActivityService = IoC.Resolve<INetworkActivityService>();
        private UIRefreshControl _refreshControl;
        private bool _loaded;


        public TViewModel ViewModel
        {
            get { return _viewModel; }
        }

        protected ViewModelDialogViewController(UITableViewStyle style = UITableViewStyle.Plain)
            : base(style, null, true)
        {
            NavigationItem.BackBarButtonItem = new UIBarButtonItem() { Title = string.Empty };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var loadableViewModel = _viewModel as LoadableViewModel;
            if (loadableViewModel != null)
            {
                _refreshControl = new UIRefreshControl();
                RefreshControl = _refreshControl;
                _refreshControl.ValueChanged += (s, e) => loadableViewModel.LoadCommand.ExecuteIfCan();

                loadableViewModel.LoadCommand.IsExecuting.Subscribe(x =>
                {
                    if (x)
                    {
                        NetworkActivityService.PushNetworkActive();
                    }
                    else
                    {
                        NetworkActivityService.PopNetworkActive();
                        _refreshControl.EndRefreshing(); 
                    }
                });
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (!_loaded)
            {
                _loaded = true;
                var loadableViewModel = _viewModel as LoadableViewModel;
                if (loadableViewModel != null)
                    loadableViewModel.LoadCommand.ExecuteIfCan();
            }
        }
    }
}

