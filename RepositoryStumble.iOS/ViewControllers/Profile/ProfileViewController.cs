using System;
using MonoTouch.UIKit;
using RepositoryStumble.Core.ViewModels.Profile;
using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;
using RepositoryStumble.Elements;
using System.Linq;

namespace RepositoryStumble.ViewControllers.Profile
{
    public class ProfileViewController : ViewModelPrettyDialogViewController<ProfileViewModel>
    {
        public override void ViewDidLoad()
        {
            Title = ViewModel.Username;

            base.ViewDidLoad();

            HeaderView.Text = ViewModel.Username;
            HeaderView.TextColor = UIColor.White;
            HeaderView.SubTextColor = UIColor.FromWhiteAlpha(0.9f, 1.0f);

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(Images.Gear, UIBarButtonItemStyle.Plain, 
                (s, e) => ViewModel.GoToSettingsCommand.ExecuteIfCan());

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

            var section2 = new Section();
            ViewModel.StumbleHistory.Changed.Subscribe(_ =>
                section2.Reset(ViewModel.StumbleHistory.Select(x => 
                    new RepositoryElement(x.Owner, x.Name, x.Description, x.ImageUrl, 
                        () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x)))));

            Root.Reset(section, section2);
        }

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

