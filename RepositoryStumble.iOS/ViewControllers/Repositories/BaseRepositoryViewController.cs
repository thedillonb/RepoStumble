using System;
using MonoTouch.UIKit;
using Xamarin.Utilities.ViewControllers;
using RepositoryStumble.Core.ViewModels.Repositories;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;
using System.Reactive.Linq;
using RepositoryStumble.Views;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public abstract class BaseRepositoryViewController<TViewModel> : ViewModelPrettyDialogViewController<TViewModel> where TViewModel : BaseRepositoryViewModel
	{
        private UIActionSheet _actionSheet;
		protected readonly UIBarButtonItem DislikeButton;
		protected readonly UIBarButtonItem LikeButton;

		protected static readonly UIColor SelectedColor = UIColor.FromRGB(0x4e, 0x4b, 0xbe);
		protected static readonly UIColor DeselectedColor = UIColor.FromRGB(50, 50, 50);

		protected BaseRepositoryViewController()
		{
            DislikeButton = new UIBarButtonItem(Images.ThumbDown, UIBarButtonItemStyle.Plain, (s, e) => ViewModel.DislikeCommand.ExecuteIfCan());
            DislikeButton.TintColor = UITabBar.Appearance.TintColor;

            LikeButton = new UIBarButtonItem(Images.ThumbUp, UIBarButtonItemStyle.Plain, (s, e) => ViewModel.LikeCommand.ExecuteIfCan());
            LikeButton.TintColor = UITabBar.Appearance.TintColor;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, (s, e) => ShowMore());
            NavigationItem.RightBarButtonItem.EnableIfExecutable(ViewModel.WhenAnyValue(x => x.Repository).Select(x => x != null));

            HeaderView.Text = string.Empty;
            HeaderView.TextColor = UIColor.White;
            HeaderView.SubTextColor = UIColor.FromWhiteAlpha(0.9f, 1.0f);

            ViewModel.WhenAnyValue(x => x.RepositoryIdentifier).Where(x => x != null)
                .Subscribe(x => Title = HeaderView.Text = x.Name);

            ViewModel.WhenAnyValue(x => x.Repository).Where(x => x != null).Subscribe(x =>
            {
                HeaderView.ImageUri = x.Owner.AvatarUrl;
                HeaderView.SubText = x.Name;
                ReloadData();
            });

            var split = new SplitButtonElement();
            var stars = split.AddButton("Stargazers", "-");
            var watchers = split.AddButton("Watchers", "-");
            var collaborators = split.AddButton("Contributors", "-");

            ViewModel.WhenAnyValue(x => x.CollaboratorCount).Subscribe(x => collaborators.Text = x.ToString());

            ViewModel.WhenAnyValue(x => x.Repository).Where(x => x != null).Subscribe(x =>
            {
                HeaderView.ImageUri = x.Owner.AvatarUrl;
                HeaderView.Text = x.Name;
                HeaderView.SubText = x.Description;
                stars.Text = x.StargazersCount.ToString();
                watchers.Text = x.SubscribersCount.ToString();
                ReloadData();
            });

            ViewModel.WhenAnyValue(x => x.Liked).Subscribe(x =>
            {
                if (x == null)
                {
                    DislikeButton.Image = Images.ThumbDown;
                    LikeButton.Image = Images.ThumbUp;
                }
                else if (x.Value)
                {
                    DislikeButton.Image = Images.ThumbDown;
                    LikeButton.Image = Images.ThumbUpFilled;
                }
                else
                {
                    DislikeButton.Image = Images.ThumbDownFilled;
                    LikeButton.Image = Images.ThumbUp;
                }
            });

            var webElement = new WebElement("readme");
            webElement.UrlRequested += (obj) => ViewModel.GoToUrlCommand.ExecuteIfCan(obj);
            ViewModel.WhenAnyValue(x => x.Readme).Subscribe(x =>
            {
                var view = new ReadmeRazorView { Model = x };
                webElement.Value = view.GenerateString();
            });

            var section = new Section { HeaderView = HeaderView };
            section.Add(split);

            var section2 = new Section();
            section2.Add(webElement);

            Root.Reset(section, section2);

            ToolbarItems = new [] 
            { 
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                DislikeButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 80 },
                LikeButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
            };
        }

		private void ShowMore()
		{
            _actionSheet = new UIActionSheet(ViewModel.RepositoryIdentifier.ToString());
            var show = _actionSheet.AddButton("Show in GitHub");
            var share = _actionSheet.AddButton("Share");
            _actionSheet.CancelButtonIndex = _actionSheet.AddButton("Cancel");
            _actionSheet.Clicked += (sender, e) =>
            {
                if (e.ButtonIndex == show)
                {
                    ViewModel.GoToGitHubCommand.ExecuteIfCan();
                }
                else if (e.ButtonIndex == share)
                {
                    var item = MonoTouch.Foundation.NSObject.FromObject(ViewModel.Repository.HtmlUrl);
                    var activityItems = new [] { item };
                    UIActivity[] applicationActivities = null;
                    var activityController = new UIActivityViewController(activityItems, applicationActivities);
                    PresentViewController(activityController, true, null);
                }

                _actionSheet = null;
            };

            _actionSheet.ShowFrom(NavigationItem.RightBarButtonItem, true);
		}
    }
}

