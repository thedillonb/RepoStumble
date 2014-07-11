using MonoTouch.UIKit;
using MonoTouch.Foundation;
using RepositoryStumble.Core.ViewModels.Application;
using ReactiveUI;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;

namespace RepositoryStumble.ViewControllers.Application
{
    public class SettingsViewController : ViewModelDialogViewController<SettingsViewModel>
    {
        public SettingsViewController() : base(true)
        {
            Title = "Settings";
        }

		private readonly string _description = "Repository Stumble is the best way to find new and exciting open source code! Think StumbleUpon for GitHub. Repository Stumble " +
		                                     "creates a list of repositories that match interests and allows you to go through them giving you the ability to choose whether to 'like' or 'dislike' each.";

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			var secHeader = new Section()
			{
				new MultilinedElement("Repository Stumble", _description) 
			};

			var settingsSection = new Section(string.Empty, "Liked repositories will be starred in GitHub and Disliked repositories will be unstarred.")
			{
                new BooleanElement("Sync With GitHub", ViewModel.SyncWithGitHub, x => ViewModel.SyncWithGitHub = x.Value)
			};

			var mid = new Section()
			{
				new StyledStringElement("Follow On Twitter", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://twitter.com/thedillonb"))),
				new StyledStringElement("Rate This App", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/repository-stumble-discover/id761416981?ls=1&mt=8"))),
				new StyledStringElement("App Version", NSBundle.MainBundle.InfoDictionary.ValueForKey(new NSString("CFBundleVersion")).ToString())
			};

			var sec = new Section()
			{
                new StyledStringElement("Logout", () => ViewModel.LogoutCommand.ExecuteIfCan())
			};

            Root.Add(secHeader, settingsSection, mid, sec);
		}
    }
}

