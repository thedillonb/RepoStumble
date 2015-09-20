using System;
using Foundation;
using UIKit;
using RepositoryStumble.Core.Messages;
using RepositoryStumble.Core.ViewModels.Application;
using ReactiveUI;
using System.Reactive;
using RepositoryStumble.ViewControllers.Application;
using Security;
using RepositoryStumble.Core.Services;
using Splat;

namespace RepositoryStumble
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
        public override UIWindow Window { get; set; }

        // This is the main entry point of the application.
        static void Main (string[] args)
        {
            UIApplication.Main (args, null, "AppDelegate");
        }

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            StampInstallDate();

            RxApp.DefaultExceptionHandler = Observer.Create((Exception e) =>
            {
                Locator.Current.GetService<IAlertDialogService>().Alert("Error", e.Message);
                Console.WriteLine("Exception occured: " + e.Message + " at " + e.StackTrace);
            });

            // Load the IoC
            Services.Registrations.InitializeServices(Locator.CurrentMutable);

            var viewModelViewService = Locator.Current.GetService<IViewModelViewService>();
            viewModelViewService.RegisterViewModels(typeof(IApplicationService).Assembly);
            viewModelViewService.RegisterViewModels(GetType().Assembly);

            // Install the theme
            SetupTheme();

            var startupViewController = new StartupViewController { ViewModel = new StartupViewModel(Locator.Current.GetService<IApplicationService>()) };
            startupViewController.ViewModel.View = startupViewController;

            var mainNavigationController = new UINavigationController(startupViewController) { NavigationBarHidden = true };
            MessageBus.Current.Listen<LogoutMessage>().Subscribe(_ =>
            {
                mainNavigationController.PopToRootViewController(false);
                mainNavigationController.DismissViewController(true, null);
            });

		    Window = new UIWindow(UIScreen.MainScreen.Bounds) {RootViewController = mainNavigationController};
            Window.MakeKeyAndVisible ();
			return true;
		}

        /// <summary>
        /// Record the date this application was installed (or the date that we started recording installation date).
        /// </summary>
        private static void StampInstallDate()
        {
            try
            {
                var query = new SecRecord(SecKind.GenericPassword) { Generic = NSData.FromString("repostumble_install_date") };
                SecStatusCode secStatusCode;
                SecKeyChain.QueryAsRecord(query, out secStatusCode);
                if (secStatusCode == SecStatusCode.Success) 
                    return;

                var newRec = new SecRecord(SecKind.GenericPassword)
                {
                    Label = "RepoStumble Install Date",
                    Description = "The first date RepoStumble was installed",
                    ValueData = NSData.FromString(DateTime.UtcNow.ToString()),
                    Generic = NSData.FromString("repostumble_install_date")
                };

                SecKeyChain.Add(newRec);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private static void SetupTheme()
        {
            var primaryColor = UIColor.FromRGB(0x4e, 0x4b, 0xbe);

            UIGraphics.BeginImageContext(new CoreGraphics.CGSize(1, 64f));
            primaryColor.SetFill();
            UIGraphics.RectFill(new CoreGraphics.CGRect(0, 0, 1, 64));
            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = primaryColor;
            UINavigationBar.Appearance.BackgroundColor = primaryColor;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White, Font = UIFont.SystemFontOfSize(18f) });
            UINavigationBar.Appearance.SetBackgroundImage(img, UIBarPosition.Any, UIBarMetrics.Default);

            UIToolbar.Appearance.BackgroundColor = UIColor.White;

            UITabBar.Appearance.TintColor = primaryColor;

            RepositoryStumble.ViewControllers.ViewModelPrettyDialogViewController.RefreshIndicatorColor = UIColor.White;

            UIApplication.SharedApplication.SetStatusBarHidden(false, UIStatusBarAnimation.Fade);
        }
	}
}

