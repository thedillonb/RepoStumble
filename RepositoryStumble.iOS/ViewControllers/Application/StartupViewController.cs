using MonoTouch.UIKit;
using Xamarin.Utilities.ViewControllers;
using RepositoryStumble.Core.ViewModels.Application;
using ReactiveUI;
using System;

namespace RepositoryStumble.ViewControllers.Application
{
    public class StartupViewController : ViewModelViewController<StartupViewModel>
    {
        private readonly UIImageView _backgroundImageView = new UIImageView();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.Add(_backgroundImageView);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            _backgroundImageView.Image = Xamarin.Utilities.Images.BackgroundHelper.LoadSplashImage();
            _backgroundImageView.Frame = View.Bounds;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewModel.StartupCommand.ExecuteIfCan();
        }

        public override bool ShouldAutorotate()
        {
            return true;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
                return UIInterfaceOrientationMask.Portrait | UIInterfaceOrientationMask.PortraitUpsideDown;
            return UIInterfaceOrientationMask.All;
        }

        /// <summary>
        /// A custom navigation controller specifically for iOS6 that locks the orientations to what the StartupControler's is.
        /// </summary>
        protected class CustomNavigationController : UINavigationController
        {
            readonly StartupViewController _parent;
            public CustomNavigationController(StartupViewController parent, UIViewController root) : base(root) 
            { 
                _parent = parent;
            }

            public override bool ShouldAutorotate()
            {
                return _parent.ShouldAutorotate();
            }

            public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
            {
                return _parent.GetSupportedInterfaceOrientations();
            }
        }
    }
}

