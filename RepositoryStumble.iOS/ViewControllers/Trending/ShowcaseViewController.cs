using System;
using MonoTouch.UIKit;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;

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

            this.Bind(ViewModel.Repositories, x =>
            {
                var element = new StyledMultilineElement(x.Owner + "/" + x.Name, x.Description,
                                      UITableViewCellStyle.Subtitle);
                element.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                element.Tapped += () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x);
                return element;
            });
        }
    }
}