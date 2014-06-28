using System;
using MonoTouch.UIKit;
using Xamarin.Utilities.Core.Services;

namespace RepositoryStumble.ViewControllers
{
    public abstract class WebViewController : UIViewController
    {
        protected readonly INetworkActivityService NetworkActivityService = IoC.Resolve<INetworkActivityService>();
        public UIWebView Web { get; private set; }

        protected WebViewController()
        {
            //NavigationItem.LeftBarButtonItem = new UIBarButtonItem(NavigationButton.Create(Theme.CurrentTheme.BackButton, () => NavigationController.PopViewControllerAnimated(true)));

            Web = new UIWebView {ScalesPageToFit = true};
            Web.LoadFinished += OnLoadFinished;
            Web.LoadStarted += OnLoadStarted;
            Web.LoadError += OnLoadError;
            Web.ShouldStartLoad = (w, r, n) => ShouldStartLoad(r, n);
			Web.BackgroundColor = UIColor.FromRGB(235, 235, 242);
        }

        protected virtual bool ShouldStartLoad (MonoTouch.Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (request.Url.AbsoluteString.StartsWith("app://ready"))
            {
                DOMReady();
                return false;
            }

            return true;
        }

        protected virtual void DOMReady()
        {
        }

        protected virtual void OnLoadError (object sender, UIWebErrorArgs e)
        {
            NetworkActivityService.PopNetworkActive();
            Console.WriteLine("Unable to load web view: " + e.Error);
        }

        protected virtual void OnLoadStarted (object sender, EventArgs e)
        {
            NetworkActivityService.PushNetworkActive();

        }

        protected virtual void OnLoadFinished(object sender, EventArgs e)
        {
            NetworkActivityService.PopNetworkActive();
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Add(Web);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            Web.Frame = View.Bounds;
        }

        protected string LoadFile(string path)
        {
			var uri = Uri.EscapeUriString("file://" + path) + "#" + Environment.TickCount;
			Web.LoadRequest(new MonoTouch.Foundation.NSUrlRequest(new MonoTouch.Foundation.NSUrl(uri)));
            return uri;
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var bounds = View.Bounds;
            bounds.Height -= NavigationController.Toolbar.Frame.Height;
            Web.Frame = bounds;
        }
        
        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            Web.Frame = View.Bounds;
        }
    }
}

