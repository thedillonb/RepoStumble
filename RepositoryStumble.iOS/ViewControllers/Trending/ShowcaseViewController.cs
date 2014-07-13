using System;
using MonoTouch.UIKit;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;
using RepositoryStumble.Elements;

namespace RepositoryStumble.ViewControllers.Trending
{
    public class ShowcaseViewController : ViewModelCollectionViewController<ShowcaseViewModel>
    {
        public ShowcaseViewController()
            : base(true, false)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.WhenAnyValue(x => x.Title).Subscribe(x => Title = x);

            this.BindList(ViewModel.Repositories, x =>
            {
                return new RepositoryElement(x.Owner, x.Name, x.Description, x.AvatarUrl, () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x));
            });
        }
    }
}