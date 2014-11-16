using System;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;
using Xamarin.Utilities.ViewControllers;
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
                new RepositoryElement(x.Owner.Login, x.Name, x.Description, x.Owner.AvatarUrl, () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x)));
        }
    }
}