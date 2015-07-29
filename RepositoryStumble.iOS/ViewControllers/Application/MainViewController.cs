using System;
using UIKit;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Application;
using RepositoryStumble.Core.ViewModels.Interests;
using RepositoryStumble.Core.ViewModels.Profile;
using RepositoryStumble.Core.ViewModels.Trending;
using RepositoryStumble.ViewControllers.Interests;
using RepositoryStumble.ViewControllers.Trending;
using CoreGraphics;
using RepositoryStumble.ViewControllers.Profile;
using RepositoryStumble.Core.Services;
using System.Linq;
using Splat;

namespace RepositoryStumble.ViewControllers.Application
{
    public class MainViewController : UITabBarController, IViewFor<MainViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var na = Locator.Current.GetService<INetworkActivityService>();
            var a = Locator.Current.GetService<IApplicationService>();
            var feat = Locator.Current.GetService<IFeaturesService>();

            var profileViewModel = new ProfileViewModel(a, na, feat);
            var profileViewController = new ProfileViewController { ViewModel = profileViewModel };
            profileViewController.ViewModel.GoToInterestsCommand.Subscribe(_ => SelectedIndex = 1);
            profileViewController.TabBarItem = new UITabBarItem("Profile", Images.User, Images.UserFilled);
            profileViewController.ViewModel.View = profileViewController;

            var interestsViewModel = new InterestsViewModel(a, feat);
            var interestsViewController = new InterestsViewController { ViewModel = interestsViewModel };
            interestsViewController.TabBarItem = new UITabBarItem("Interests", Images.Heart, Images.HeartFilled);
            interestsViewController.ViewModel.View = interestsViewController;

            var trendingViewModel = new TrendingViewModel(na, new RepositoryStumble.Core.Data.TrendingRepository());
            var trendingViewController = new TrendingViewController();
            trendingViewController.ViewModel = trendingViewModel;
            trendingViewController.TabBarItem = new UITabBarItem("Trending", Images.Trending, Images.TrendingFilled);
            trendingViewController.ViewModel.View = trendingViewController;

            var vm = new ShowcasesViewModel(Locator.Current.GetService<INetworkActivityService>(), new RepositoryStumble.Core.Data.ShowcaseRepository());
            var showcasesViewController = new ShowcasesViewController { ViewModel = vm };
            showcasesViewController.TabBarItem = new UITabBarItem("Showcase", Images.Spotlight, Images.Spotlight);
            showcasesViewController.ViewModel.View = showcasesViewController;

            var stumble = new UIViewController();
            stumble.TabBarItem = new UITabBarItem("Stumble!", Images.Search, Images.Search);

            this.ViewControllers = new[]
            {
                new UINavigationController(profileViewController),
                new UINavigationController(interestsViewController),
                stumble,
                new UINavigationController(trendingViewController),
                new UINavigationController(showcasesViewController)
            };

            nfloat width = 60;
            if (TabBar.Subviews.Length == 5)
                width = TabBar.Subviews[2].Bounds.Width;

            var stumbleView = new StumbleView(new CGRect(TabBar.Bounds.Width / 2f - width / 2, 0, width, TabBar.Bounds.Height), UITabBar.Appearance.TintColor);
            stumbleView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            stumbleView.UserInteractionEnabled = false;
            TabBar.Add(stumbleView);

            nint previousSelectedIndex = 0;
            ViewControllerSelected += (sender, e) =>
            {
                if (e.ViewController == stumble)
                {
                    if (Locator.Current.GetService<IApplicationService>().Account.Interests.Count() == 0)
                    {
                        SelectedIndex = 1;
                        Locator.Current.GetService<IAlertDialogService>().Alert(
                            "You need some interests!", 
                            "Please add at least one interest before you stumble!");
                    }
                    else
                    {
                        ViewModel.GoToStumbleCommand.ExecuteIfCan();
                        SelectedIndex = previousSelectedIndex;
                    }
                }
                else
                {
                    previousSelectedIndex = SelectedIndex;
                }
            };
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }

        public MainViewModel ViewModel { get; set; }

        private class StumbleView : UIView
        {
            public StumbleView(CGRect rect, UIColor backgroundColor)
                : base(rect)
            {
                var bgWidth = rect.Width;
                var mask = UIViewAutoresizing.FlexibleWidth  | UIViewAutoresizing.FlexibleHeight;
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    bgWidth = 90f;
                    mask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleHeight;
                }

                var background = new UIView(new CGRect(0, 0, bgWidth, rect.Height));
                background.AutoresizingMask = mask;
                background.BackgroundColor = backgroundColor;
                background.Center = new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());
                Add(background);

                var img = new UIImageView(new CGRect(rect.Width / 2f - 14f, 4f, 28f, 28f));
                img.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
                img.Image = Images.Search.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                img.ContentMode = UIViewContentMode.ScaleAspectFit;
                img.TintColor = UIColor.White;
                Add(img);

                var lbl = new UILabel(new CGRect(0, rect.Height - 12f, rect.Width, 10f));
                lbl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin;
                lbl.Font = UIFont.SystemFontOfSize(10f);
                lbl.TextAlignment = UITextAlignment.Center;
                lbl.TextColor = UIColor.White;
                lbl.Text = "Stumble!";
                Add(lbl);
            }
        }
    }
}

