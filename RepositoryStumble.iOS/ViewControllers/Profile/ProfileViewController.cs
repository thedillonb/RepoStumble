using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Linq;
using RepositoryStumble.Views;
using MonoTouch.Dialog.Utilities;
using RepositoryStumble.Core.ViewModels.Profile;

namespace RepositoryStumble.ViewControllers
{
    public class ProfileViewController : ViewModelDialogViewController<ProfileViewModel>, IImageUpdated
    {
		private readonly HeaderView _header = new HeaderView();

        public ProfileViewController()
			: base(UITableViewStyle.Grouped)
        {
            Title = "Profile";
//			NavigationItem.RightBarButtonItem = new UIBarButtonItem(Images.Gear, UIBarButtonItemStyle.Plain, (s, e) =>
//				NavigationController.PushViewController(new SettingsViewController(), true));
        }

		public override void ViewWillAppear(bool animated)
		{
//			NavigationController.Toolbar.Translucent = false;
//			NavigationController.Toolbar.BarTintColor = UIColor.FromRGB(245, 245, 245);
//			base.ViewWillAppear(animated);
//
//			var repos = Application.Instance.Account.StumbledRepositories;
//			var total = repos.Where(x => x.ShowInHistory).Count();
//			var likes = repos.Where(x => x.Liked != null && x.Liked.Value).Count();
//			var dislikes = repos.Where(x => x.Liked != null && !x.Liked.Value).Count();
//			var interests = Application.Instance.Account.Interests.Count();
//
//			var secHeader = new Section(null, _header);
//			_header.Title = Application.Instance.Account.Username;
//			_header.Subtitle = Application.Instance.Account.Fullname;
//			_header.Image = ImageLoader.DefaultRequestImage(new System.Uri(Application.Instance.Account.AvatarUrl), this);
//
//			var sec1 = new Section()
//			{
//				new Element("Likes", likes, () => NavigationController.PushViewController(new LikedViewController(), true)),
//				new Element("Dislikes", dislikes, () => NavigationController.PushViewController(new DislikedViewController(), true)),
//				new Element("History", total, () => NavigationController.PushViewController(new HistoryViewController(), true)),
//			};
//
//			var sec2 = new Section()
//			{
//				new Element("Interests", interests, () => NavigationController.PushViewController(new InterestsViewController(), true)),
//			};
//
//			var sec3 = new Section()
//			{
//				new StyledStringElement("Settings", () => NavigationController.PushViewController(new SettingsViewController(), true))
//			};
//
//			Root = new RootElement("Repository Stumble") { secHeader, sec2, sec1, sec3 };
		}

		public void UpdatedImage (System.Uri uri)
		{
			_header.Image = ImageLoader.DefaultRequestImage(uri, this);
			if (_header.Image != null)
				_header.SetNeedsDisplay();
		}

		private class Element : StyledStringElement
		{
			public Element(string caption, object value, MonoTouch.Foundation.NSAction action)
				: base(caption, action)
			{
				this.style = UITableViewCellStyle.Value1;
				this.Value = value.ToString();
			}
		}
    }
}

