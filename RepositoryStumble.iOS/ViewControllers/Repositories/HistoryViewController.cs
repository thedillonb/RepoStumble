using MonoTouch.UIKit;
using RepositoryStumble.Core.ViewModels.Repositories;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public class HistoryViewController : BaseRepositoriesViewController<HistoryViewModel>
	{
        public HistoryViewController()
        {
            Title = "History";
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, (s, e) => ShowSettings());
        }

		private void ShowSettings()
		{
//			var a = MonoTouch.Utilities.GetSheet(null);
//			var clearButton = a.AddButton("Clear All");
//			a.CancelButtonIndex = a.AddButton("Cancel");
//
//			a.Clicked += (object sender, UIButtonEventArgs e) => {
//				if (e.ButtonIndex == clearButton)
//				{
//					Application.Instance.Account.StumbledRepositories.MarkAllAsNotInHistory();
//					Reload();
//				}
//			};
//
//			a.ShowFrom(NavigationItem.RightBarButtonItem, true);
//
//			foreach (var v in a.Subviews)
//			{
//				var btn = v as UIButton;
//				if (btn != null && string.Equals(btn.Title(UIControlState.Normal), "Cancel"))
//					btn.SetTitleColor(UIColor.Red, UIControlState.Normal);
//			}
		}
	}
}

