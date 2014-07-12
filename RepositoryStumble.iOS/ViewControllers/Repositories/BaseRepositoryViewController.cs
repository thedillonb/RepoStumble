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
		protected readonly UIBarButtonItem DislikeButton;
		protected readonly UIBarButtonItem LikeButton;

		protected static readonly UIColor SelectedColor = UIColor.FromRGB(0x4e, 0x4b, 0xbe);
		protected static readonly UIColor DeselectedColor = UIColor.FromRGB(50, 50, 50);

		protected BaseRepositoryViewController()
		{
			NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, (s, e) => ShowMore());

            DislikeButton = new UIBarButtonItem(Images.ThumbDown, UIBarButtonItemStyle.Plain, (s, e) => ViewModel.DislikeCommand.ExecuteIfCan());
            DislikeButton.TintColor = UITabBar.Appearance.TintColor;

            LikeButton = new UIBarButtonItem(Images.ThumbUp, UIBarButtonItemStyle.Plain, (s, e) => ViewModel.LikeCommand.ExecuteIfCan());
            LikeButton.TintColor = UITabBar.Appearance.TintColor;

        }
            

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            HeaderView.Text = string.Empty;
            HeaderView.TextColor = UIColor.White;
            HeaderView.SubTextColor = UIColor.FromWhiteAlpha(0.9f, 1.0f);
            Title = HeaderView.Text = ViewModel.RepositoryIdentifier.Name;

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
//
//			var a = MonoTouch.Utilities.GetSheet(null);
//			var show = a.AddButton("Show in GitHub");
//			var share = a.AddButton("Share");
//			a.CancelButtonIndex = a.AddButton("Cancel");
//
//			a.Clicked += (object sender, UIButtonEventArgs e) =>
//			{
//				if (e.ButtonIndex == show)
//				{
//					try { UIApplication.SharedApplication.OpenUrl(new MonoTouch.Foundation.NSUrl(CreateUrl())); } catch { }
//				}
//				else if (e.ButtonIndex == share)
//				{
//					var item = UIActivity.FromObject (CreateUrl());
//					var activityItems = new MonoTouch.Foundation.NSObject[] { item };
//					UIActivity[] applicationActivities = null;
//					var activityController = new UIActivityViewController (activityItems, applicationActivities);
//					PresentViewController (activityController, true, null);
//				}
//			};
//
//			a.ShowFrom(NavigationItem.RightBarButtonItem, true);
		}

//		protected override bool ShouldStartLoad(MonoTouch.Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
//		{
//			if (request.Url.AbsoluteString.StartsWith("http"))
//			{
////				var ctrl = new WebBrowserViewController();
////				ctrl.Title = Title;
////				ctrl.Load(request.Url);
////				NavigationController.PushViewController(ctrl, true);
//				return false;
//			}
//
//			return base.ShouldStartLoad(request, navigationType);
//		}

		protected virtual void Like()
		{
//			CurrentRepo.Liked = true;
//			Application.Instance.Account.StumbledRepositories.Update(CurrentRepo);
//			BigTed.BTProgressHUD.ShowSuccessWithStatus("Liked!");
//			_likeButton.TintColor = SelectedColor;
//			_dislikeButton.TintColor = DeselectedColor;
//
//			if (Application.Instance.Account.SyncWithGitHub)
//			{
//				var req = Application.Instance.Client.Users[CurrentRepo.Owner].Repositories[CurrentRepo.Name].Star();
//				Application.Instance.Client.ExecuteAsync(req);
//			}
		}

		protected virtual void Dislike()
		{
//			CurrentRepo.Liked = false;
//			Application.Instance.Account.StumbledRepositories.Update(CurrentRepo);
//			BigTed.BTProgressHUD.ShowErrorWithStatus("Disliked!");
//			_dislikeButton.TintColor = SelectedColor;
//			_likeButton.TintColor = DeselectedColor;
//
//			if (Application.Instance.Account.SyncWithGitHub)
//			{
//				var req = Application.Instance.Client.Users[CurrentRepo.Owner].Repositories[CurrentRepo.Name].Unstar();
//				Application.Instance.Client.ExecuteAsync(req);
//			}
		}
    }
}

