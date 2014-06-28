using System;
using MonoTouch.UIKit;
using RepositoryStumble.ViewControllers.Interests;
using RepositoryStumble.ViewControllers.Trending;

namespace RepositoryStumble.ViewControllers.Application
{
    public class MainViewController : UITabBarController
    {
        public MainViewController()
        {
            var profileViewController = new ProfileViewController();
            var interestsViewController = new InterestsViewController();
            var showcasesViewController = new ShowcasesViewController();

            this.ViewControllers = new UIViewController[]
            {
                new UINavigationController(profileViewController),
                new UINavigationController(interestsViewController),
                new UIViewController() { Title = "Stumble!" },
                new UINavigationController(showcasesViewController)
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
    }
}

