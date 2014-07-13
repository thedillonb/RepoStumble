using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RepositoryStumble.Core.Messages;
using RepositoryStumble.Core.ViewModels.Application;
using MTiRate;
using Parse;
using ReactiveUI;
using System.Reactive.Concurrency;
using System.Threading;
using System.Reactive;
using Xamarin.Utilities.Core.Services;
using RepositoryStumble.ViewControllers.Application;

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
			// Initialize the Parse client with your Application ID and .NET Key found on
			// your Parse dashboard
			ParseClient.Initialize("BNE1MlfKES62wVbAkEhBuVL5sxSsTbWtmQAp4fNl",
				"rPcGNYwXJqOpNjeP0tMBHCf4d7oUpnUFhPUosfSQ");

			ParseAnalytics.TrackAppOpenedAsync();

            RxApp.MainThreadScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
            RxApp.DefaultExceptionHandler = Observer.Create((Exception e) =>
            {
                IoC.Resolve<IAlertDialogService>().Alert("Error", e.Message);
                Console.WriteLine("Exception occured: " + e.Message + " at " + e.StackTrace);
            });

            // Load the IoC
            IoC.RegisterAssemblyServicesAsSingletons(typeof(Xamarin.Utilities.Core.Services.IDefaultValueService).Assembly);
            IoC.RegisterAssemblyServicesAsSingletons(typeof(Xamarin.Utilities.Services.DefaultValueService).Assembly);
            IoC.RegisterAssemblyServicesAsSingletons(typeof(Core.Services.IApplicationService).Assembly);
            IoC.RegisterAssemblyServicesAsSingletons(GetType().Assembly);

            var viewModelViewService = IoC.Resolve<IViewModelViewService>();
            viewModelViewService.RegisterViewModels(typeof(Xamarin.Utilities.Services.DefaultValueService).Assembly);
            viewModelViewService.RegisterViewModels(typeof(Core.Services.IApplicationService).Assembly);
            viewModelViewService.RegisterViewModels(GetType().Assembly);

			iRate.SharedInstance.AppStoreID = 761416981;
			iRate.SharedInstance.ApplicationBundleID = "com.dillonbuchanan.repositorystumble";
			iRate.SharedInstance.DaysUntilPrompt = 2;
			iRate.SharedInstance.UsesUntilPrompt = 5;
			iRate.SharedInstance.OnlyPromptIfLatestVersion = true;

            // Install the theme
            SetupTheme();

            //GitHubSharp.Client.ClientConstructor = () => new System.Net.Http.HttpClient(new ModernHttpClient.AFNetworkHandler());
            var startupViewController = new StartupViewController { ViewModel = IoC.Resolve<StartupViewModel>() };
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

        private static void SetupTheme()
        {
            var primaryColor = UIColor.FromRGB(0x4e, 0x4b, 0xbe);

            UIGraphics.BeginImageContext(new System.Drawing.SizeF(1, 64f));
            primaryColor.SetFill();
            UIGraphics.RectFill(new System.Drawing.RectangleF(0, 0, 1, 64));
            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = primaryColor;
            UINavigationBar.Appearance.BackgroundColor = primaryColor;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White, Font = UIFont.SystemFontOfSize(18f) });
            UINavigationBar.Appearance.SetBackgroundImage(img, UIBarPosition.Any, UIBarMetrics.Default);

            UITabBar.Appearance.TintColor = primaryColor;

            Xamarin.Utilities.ViewControllers.ViewModelPrettyDialogViewController.RefreshIndicatorColor = UIColor.White;

            UIApplication.SharedApplication.SetStatusBarHidden(false, UIStatusBarAnimation.Fade);
        }
	}
}

