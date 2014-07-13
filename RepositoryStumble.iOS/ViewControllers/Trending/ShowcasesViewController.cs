using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;
using Xamarin.Utilities.ViewControllers;
using RepositoryStumble.Elements;

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

            this.BindList(ViewModel.Showcases, x => new ShowcaseElement(x.Name, x.Description, 
                "http://trending.codehub-app.com/" + x.Image, () => ViewModel.GoToShowcaseCommand.ExecuteIfCan(x)));
        }
    }
}

