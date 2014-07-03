using System;
using RepositoryStumble.Core.ViewModels.Repositories;
using MonoTouch.Dialog;
using RepositoryStumble.Elements;
using System.Linq;
using RepositoryStumble.ViewControllers.Stumble;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;
using MonoTouch.UIKit;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public abstract class BaseRepositoriesViewController<TViewModel> : ViewModelDialogViewController<TViewModel> where TViewModel : BaseRepositoriesViewModel
    {
        protected BaseRepositoriesViewController()
            : base(unevenRows: true, style: UITableViewStyle.Plain)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.Repositories.Changed.Subscribe(_ =>
            {
                var sec = new Section();
                sec.AddAll(from x in ViewModel.Repositories
                    select new StumbledRepositoryElement(x, () => {
                        var ctrl = new SeenStumbleViewController(x);
                        NavigationController.PushViewController(ctrl, true);
                    }));
                Root.Reset(sec);
            });
        }
    }
}

