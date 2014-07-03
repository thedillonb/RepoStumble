using MonoTouch.UIKit;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;

namespace RepositoryStumble.ViewControllers.Trending
{
    public class ShowcasesViewController : ViewModelCollectionViewController<ShowcasesViewModel>
    {
        public ShowcasesViewController()
            : base(true, false)
        {
            Title = "Showcases";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Bind(ViewModel.Showcases, x =>
            {
                var e = new StyledMultilineElement(x.Name, x.Description, UITableViewCellStyle.Subtitle);
                e.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                e.Tapped += () => ViewModel.GoToShowcaseCommand.ExecuteIfCan(x);
                return e;
            });
        }
    }
}

