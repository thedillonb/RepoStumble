using System;
using RepositoryStumble.Core.ViewModels.Application;
using ReactiveUI;
using Xamarin.Utilities.ViewControllers;
using System.Text;
using Xamarin.Utilities.Core.Services;
using System.Reactive.Linq;

namespace RepositoryStumble.ViewControllers.Application
{
    public class LoginViewController : WebView<LoginViewModel>
    {
        private readonly IStatusIndicatorService _statusIndicator;
        public LoginViewController(IStatusIndicatorService statusIndicator)
        {
            Title = "Login";
            _statusIndicator = statusIndicator;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.LoginCommand.IsExecuting.Subscribe(x =>
            {
                if (x)
                    _statusIndicator.Show("Logging in...");
                else
                    _statusIndicator.Hide();
            });

            foreach (var c in MonoTouch.Foundation.NSHttpCookieStorage.SharedStorage.Cookies)
                MonoTouch.Foundation.NSHttpCookieStorage.SharedStorage.DeleteCookie(c);
            MonoTouch.Foundation.NSUrlCache.SharedCache.RemoveAllCachedResponses();
            Web.LoadRequest(new MonoTouch.Foundation.NSUrlRequest(new MonoTouch.Foundation.NSUrl(ViewModel.LoginUrl)));
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

            //Apple is full of clowns. The GitHub login page has links that can ultimiately end you at a place where you can purchase something
            //so we need to inject javascript that will remove these links. What a bunch of idiots...
            var script = new StringBuilder();
            script.Append("$('.switch-to-desktop').hide();");
            script.Append("$('.header-button').hide();");
            script.Append("$('.header').hide();");
            script.Append("$('.site-footer').hide();");
            script.Append("$('.brand-logo-wordmark').click(function(e) { e.preventDefault(); });");
            Web.EvaluateJavascript("(function(){setTimeout(function(){" + script +"}, 100); })();");
        }
    }
}

