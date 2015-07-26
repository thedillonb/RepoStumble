using System.Threading.Tasks;
using UIKit;
using BigTed;
using RepositoryStumble.Core.Services;

namespace UIKit
{
    public static class UIWindowExtensions
    {
        public static UIViewController GetVisibleViewController(this UIWindow @this)
        {
            var topViewController = @this.RootViewController;
            while (topViewController.PresentedViewController != null)
                topViewController = topViewController.PresentedViewController;
            return topViewController;
        }
    }
}


namespace RepositoryStumble.Services
{
    public class AlertDialogService : IAlertDialogService
    {
        private static UIViewController ViewController
        {
            get { return UIApplication.SharedApplication.KeyWindow.GetVisibleViewController(); }
        }

        public Task<bool> PromptYesNo(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Cancel, x => tcs.SetResult(false)));
            alert.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, x => tcs.SetResult(true)));
            ViewController.PresentViewController(alert, true, null);
            return tcs.Task;
        }

        public Task Alert(string title, string message)
        {
            var tcs = new TaskCompletionSource<object>();
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, x => tcs.SetResult(true)));
            ViewController.PresentViewController(alert, true, null);
            return tcs.Task;
        }

        public Task<string> PromptTextBox(string title, string message, string defaultValue, string okTitle)
        {
            var tcs = new TaskCompletionSource<string>();
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            alert.AddTextField(t => {
                t.Text = defaultValue;
            });

            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, x => tcs.SetCanceled()));
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, x => tcs.SetResult(alert.TextFields[0].Text)));
            ViewController.PresentViewController(alert, true, null);
            return tcs.Task;
        }

        public static UIColor BackgroundTint;

        public void Show(string text)
        {
            ProgressHUD.Shared.HudBackgroundColour = BackgroundTint;
            BTProgressHUD.Show(text, maskType: ProgressHUD.MaskType.Gradient);
        }

        public void ShowSuccess(string text)
        {
            BTProgressHUD.ShowSuccessWithStatus(text);
        }

        public void ShowError(string text)
        {
            BTProgressHUD.ShowErrorWithStatus(text);
        }

        public void Hide()
        {
            BTProgressHUD.Dismiss();
        }
    }
}

