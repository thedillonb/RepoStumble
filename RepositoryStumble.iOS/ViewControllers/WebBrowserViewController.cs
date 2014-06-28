using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace RepositoryStumble.ViewControllers
{
	public class WebBrowserViewController : WebViewController
    {
		private readonly UIBarButtonItem _backButton;
		private readonly UIBarButtonItem _forwardButton;

        public WebBrowserViewController()
        {
			ToolbarItems = new MonoTouch.UIKit.UIBarButtonItem[]
			{
				new MonoTouch.UIKit.UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 10f },
				(_backButton = new MonoTouch.UIKit.UIBarButtonItem(Images.Back, UIBarButtonItemStyle.Plain, (s, e) => Back())),
				new MonoTouch.UIKit.UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 20f },
				(_forwardButton = new MonoTouch.UIKit.UIBarButtonItem(Images.Forward, UIBarButtonItemStyle.Plain, (s, e) => Forward())),
				new MonoTouch.UIKit.UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new MonoTouch.UIKit.UIBarButtonItem(Images.Reload, UIBarButtonItemStyle.Plain, (s, e) => Refresh()),
				new MonoTouch.UIKit.UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 10f },
			};

			_backButton.Enabled = false;
			_forwardButton.Enabled = false;
        }

		protected override void OnLoadFinished(object sender, EventArgs e)
		{
			base.OnLoadFinished(sender, e);

			_backButton.Enabled = Web.CanGoBack;
			_forwardButton.Enabled = Web.CanGoForward;
		}

		protected override void OnLoadError(object sender, UIWebErrorArgs e)
		{
			base.OnLoadError(sender, e);
			_backButton.Enabled = Web.CanGoBack;
			_forwardButton.Enabled = Web.CanGoForward;
		}

		public void Load(NSUrl url)
		{
			Web.LoadRequest(new MonoTouch.Foundation.NSUrlRequest(url));
		}

		private void Back()
		{
			if (Web.CanGoBack)
				Web.GoBack();
			_backButton.Enabled = Web.CanGoBack;
			_forwardButton.Enabled = Web.CanGoForward;
		}

		private void Forward()
		{
			if (Web.CanGoForward)
				Web.GoForward();
			_backButton.Enabled = Web.CanGoBack;
			_forwardButton.Enabled = Web.CanGoForward;
		}

		private void Refresh()
		{
			Web.Reload();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.SetToolbarHidden(false, true);
		}
    }
}

