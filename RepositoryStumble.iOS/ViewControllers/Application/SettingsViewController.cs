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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var settingsSection = new Section(string.Empty, "Liked repositories will be starred in GitHub and Disliked repositories will be unstarred.")
            {
                new BooleanElement("Sync With GitHub", ViewModel.SyncWithGitHub, x => ViewModel.SyncWithGitHub = x.Value)
            };

            var mid = new Section()
            {
                new StyledStringElement("Follow On Twitter", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://twitter.com/thedillonb"))),
                new StyledStringElement("Rate This App", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/repository-stumble-discover/id761416981?ls=1&mt=8"))),
                new StyledStringElement("Source Code", () => ViewModel.GoToSourceCode.ExecuteIfCan()),
                new StyledStringElement("App Version", ViewModel.Version)
            };

            var sec = new Section()
            {
                new StyledStringElement("Logout", () => ViewModel.LogoutCommand.ExecuteIfCan())
            };

            Root.Reset(settingsSection, mid, sec);
        }
    }
}

