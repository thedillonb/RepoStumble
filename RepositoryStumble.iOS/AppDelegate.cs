using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RepositoryStumble.ViewControllers;
using RepositoryStumble.Utils;
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
                IoC.Resolve<IAlertDialogService>().Alert("Unhandled Exception", e.Message);
                Console.WriteLine("Exception occured: " + e.Message + " at " + e.StackTrace);
            });

            // Load the IoC
            IoC.RegisterAssemblyServicesAsSingletons(typeof(Xamarin.Utilities.Core.Services.IDefaultValueService).Assembly);
            IoC.RegisterAssemblyServicesAsSingletons(typeof(Xamarin.Utilities.Services.DefaultValueService).Assembly);
            IoC.RegisterAssemblyServicesAsSingletons(typeof(Core.Services.IApplicationService).Assembly);
            IoC.RegisterAssemblyServicesAsSingletons(GetType().Assembly);

			iRate.SharedInstance.AppStoreID = 761416981;
			iRate.SharedInstance.ApplicationBundleID = "com.dillonbuchanan.repositorystumble";
			iRate.SharedInstance.DaysUntilPrompt = 2;
			iRate.SharedInstance.UsesUntilPrompt = 5;
			iRate.SharedInstance.OnlyPromptIfLatestVersion = true;

            //GitHubSharp.Client.ClientConstructor = () => new System.Net.Http.HttpClient(new ModernHttpClient.AFNetworkHandler());

            var rootViewController = new UINavigationController(new StartupViewController()) { NavigationBarHidden = true };
            Window = new UIWindow (UIScreen.MainScreen.Bounds);
            Window.RootViewController = rootViewController;
            Window.MakeKeyAndVisible ();

            MessageBus.Current.Listen<RepositoryStumble.Core.Messages.LogoutMessage>().Subscribe(_ => rootViewController.PopToRootViewController(true));
			return true;
		}
	}
}

