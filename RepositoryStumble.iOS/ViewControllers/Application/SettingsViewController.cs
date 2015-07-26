using UIKit;
using Foundation;
using RepositoryStumble.Core.ViewModels.Application;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;

namespace RepositoryStumble.ViewControllers.Application
{
    public class SettingsViewController : ViewModelDialogViewController<SettingsViewModel>
    {
        public SettingsViewController() : base(true)
        {
            Title = "Settings";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var settingsSection = new Section(string.Empty, "Liked repositories will be starred in GitHub and Disliked repositories will be unstarred.")
            {
                new BooleanElement("Sync With GitHub", ViewModel.SyncWithGitHub, x => ViewModel.SyncWithGitHub = x.Value)
            };

            var mid = new Section()
            {
                new StringElement("Follow On Twitter", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://twitter.com/thedillonb"))),
                new StringElement("Rate This App", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/repository-stumble-discover/id761416981?ls=1&mt=8"))),
                new StringElement("Source Code", () => ViewModel.GoToSourceCode.ExecuteIfCan()),
                new StringElement("App Version", ViewModel.Version)
            };

            var sec = new Section()
            {
                new StringElement("Logout", () => ViewModel.LogoutCommand.ExecuteIfCan())
            };

            Root.Reset(settingsSection, mid, sec);
        }
    }
}

