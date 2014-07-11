using System;
using MonoTouch.UIKit;
using RepositoryStumble.Core.ViewModels.Profile;
using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;

namespace RepositoryStumble.ViewControllers.Profile
{
    public class ProfileViewController : ViewModelPrettyDialogViewController<ProfileViewModel>
    {
        public ProfileViewController()
        {
            Title = "Profile";

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(Images.Gear, UIBarButtonItemStyle.Plain, 
                (s, e) => ViewModel.GoToSettingsCommand.ExecuteIfCan());
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            HeaderView.Text = ViewModel.Username;
            HeaderView.TextColor = UIColor.White;
            HeaderView.SubTextColor = UIColor.FromWhiteAlpha(0.9f, 1.0f);

            ViewModel.WhenAnyValue(x => x.User).Where(x => x != null).Subscribe(x =>
            {
                HeaderView.ImageUri = x.AvatarUrl;
                HeaderView.SubText = x.Name;
                ReloadData();
            });

            var split = new SplitButtonElement();
            var likes = split.AddButton("Likes", "-", () => ViewModel.GoToLikesCommand.ExecuteIfCan());
            var dislikes = split.AddButton("Dislikes", "-", () => ViewModel.GoToDislikesCommand.ExecuteIfCan());
            var interests = split.AddButton("Interests", "-", () => ViewModel.GoToInterestsCommand.ExecuteIfCan());

            ViewModel.WhenAnyValue(x => x.Likes).Subscribe(x => likes.Text = x.ToString());
            ViewModel.WhenAnyValue(x => x.Dislikes).Subscribe(x => dislikes.Text = x.ToString());
            ViewModel.WhenAnyValue(x => x.Interests).Subscribe(x => interests.Text = x.ToString());

            var section = new Section { HeaderView = HeaderView };
            section.Add(split);

            Root.Reset(section);
        }

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

		private class Element : StyledStringElement
		{
			public Element(string caption, object value, Action action)
				: base(caption, action)
			{
				this.style = UITableViewCellStyle.Value1;
				this.Value = value.ToString();
			}
		}
    }
}

