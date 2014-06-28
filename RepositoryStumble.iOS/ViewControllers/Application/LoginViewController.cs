using System;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using RepositoryStumble.Core.ViewModels.Application;
using ReactiveUI;

namespace RepositoryStumble.ViewControllers.Application
{
    public class LoginViewController : WebViewController
    {
        protected readonly LoginViewModel ViewModel = IoC.Resolve<LoginViewModel>();

        public LoginViewController()
        {
            Title = "Login";

            LoadRequest();

			ToolbarItems = new UIBarButtonItem[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(Images.Back, UIBarButtonItemStyle.Plain, (s, e) => Back()),
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(Images.Forward, UIBarButtonItemStyle.Plain, (s, e) => Forward()),
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(Images.Reload, UIBarButtonItemStyle.Plain, (s, e) => Reload()),
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
			};

            ViewModel.LoginCommand.IsExecuting.Subscribe(x =>
            {
            });
        }

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.Default;
		}

		private void Back()
		{
			if (Web.CanGoBack)
				Web.GoBack();
		}

		private void Forward()
		{
			if (Web.CanGoForward)
				Web.GoForward();
		}

		private void Reload()
		{
			Web.Reload();
		}

		protected override bool ShouldStartLoad(MonoTouch.Foundation.NSUrlRequest request, MonoTouch.UIKit.UIWebViewNavigationType navigationType)
        {
            Console.WriteLine("Attemping to load: " + request.Url);

            //We're being redirected to our redirect URL so we must have been successful
            if (request.Url.Host == "dillonbuchanan.com")
            {
                var code = request.Url.Query.Split('=')[1];
                ViewModel.LoginCode = code;
                ViewModel.LoginCommand.ExecuteIfCan();
                return false;
            }
            return base.ShouldStartLoad(request, navigationType);
        }

        protected override void OnLoadFinished(object sender, EventArgs e)
        {
            base.OnLoadFinished(sender, e);

//            //Inject some Javascript so we can set the username if there is an attempted account
//            if (_attemptedAccount != null)
//            {
//                var script = "(function() { setTimeout(function() { $('input[name=\"login\"]').val('" + _attemptedAccount.Username + "').attr('readonly', 'readonly'); }, 100); })();";
//                Web.EvaluateJavascript(script);
//            }
        }

        private void LoadRequest()
        {
            //Remove all cookies & cache
            foreach (var c in MonoTouch.Foundation.NSHttpCookieStorage.SharedStorage.Cookies)
                MonoTouch.Foundation.NSHttpCookieStorage.SharedStorage.DeleteCookie(c);
            MonoTouch.Foundation.NSUrlCache.SharedCache.RemoveAllCachedResponses();
            Web.LoadRequest(new MonoTouch.Foundation.NSUrlRequest(new MonoTouch.Foundation.NSUrl(ViewModel.LoginUrl)));
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, animated);
		}
    }
}

