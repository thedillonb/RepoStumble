using System;
using RepositoryStumble.Core.ViewModels.Repositories;
using MonoTouch.Dialog;
using RepositoryStumble.Elements;
using System.Linq;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public class BaseRepositoriesViewController<TViewModel> : ViewModelDialogViewController<TViewModel> where TViewModel : BaseRepositoriesViewModel
    {
        public BaseRepositoriesViewController()
        {
            var viewModel = (BaseRepositoriesViewModel)ViewModel;
            viewModel.Repositories.Changed.Subscribe(_ =>
            {
                var sec = new Section();
                sec.AddAll(from x in viewModel.Repositories
                    select new StumbledRepositoryElement(x, () => {
                        var ctrl = new SeenStumbleViewController(x);
                        NavigationController.PushViewController(ctrl, true);
                    }));
                Root = new RootElement(Title) { sec };
                Root.UnevenRows = true;
            });
        }
    }
}

