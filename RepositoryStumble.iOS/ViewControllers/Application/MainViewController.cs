using System;
using MonoTouch.UIKit;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Application;
using RepositoryStumble.Core.ViewModels.Interests;
using RepositoryStumble.Core.ViewModels.Profile;
using RepositoryStumble.Core.ViewModels.Trending;
using RepositoryStumble.ViewControllers.Interests;
using RepositoryStumble.ViewControllers.Trending;

namespace RepositoryStumble.ViewControllers.Application
{
    public class MainViewController : UITabBarController, IViewFor<MainViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var profileViewController = new ProfileViewController { ViewModel = IoC.Resolve<ProfileViewModel>() };
            profileViewController.ViewModel.View = profileViewController;

            var interestsViewController = new InterestsViewController { ViewModel = IoC.Resolve<InterestsViewModel>() };
            interestsViewController.ViewModel.View = interestsViewController;

            var showcasesViewController = new ShowcasesViewController { ViewModel = IoC.Resolve<ShowcasesViewModel>() };
            showcasesViewController.ViewModel.View = showcasesViewController;


            this.ViewControllers = new[]
            {
                new UINavigationController(profileViewController),
                new UINavigationController(interestsViewController),
                new UIViewController() { Title = "Stumble!" },
                new UINavigationController(showcasesViewController)
                //new UINavigationController(showcasesViewController)
            };
        }

        private void GoToStumble()
        {
//            var interestsCount = Application.Instance.Account.Interests.Count();
//            if (interestsCount == 0)
//            {
//                MonoTouch.Utilities.ShowAlert("No Interests!", "You need to add some interests before you start your journey!", () =>
//                    NavigationController.PushViewController(new InterestsViewController(), true));
//                return;
//            }
//
//            if (Application.Instance.Account.Interests.Where(x => x.Exhaused).Count() == interestsCount)
//            {
//                MonoTouch.Utilities.ShowAlert("You've seen it all!", "Add more interests because you've seen everything I've got to show you for the ones you have!", () =>
//                    NavigationController.PushViewController(new InterestsViewController(), true));
//                return;
//            }
//
//            NavigationController.PushViewController(new StumbleViewController(), true);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }

        public MainViewModel ViewModel { get; set; }
    }
}

