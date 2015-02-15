using System;
using MonoTouch.UIKit;
using Xamarin.Utilities.ViewControllers;
using RepositoryStumble.Core.ViewModels.Repositories;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;
using System.Reactive.Linq;
using RepositoryStumble.Views;
using System.Drawing;
using MonoTouch.Foundation;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public abstract class BaseRepositoryViewController<TViewModel> : ViewModelPrettyDialogViewController<TViewModel> where TViewModel : BaseRepositoryViewModel
	{
        private UIActionSheet _actionSheet;
        private WebElement _webElement;
        private bool _disposed;
		protected readonly UIBarButtonItem DislikeButton;
		protected readonly UIBarButtonItem LikeButton;

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
            NavigationItem.RightBarButtonItem.EnableIfExecutable(this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.WhenAnyValue(y => y.Repository))
                .Switch().Select(x => x != null));

            HeaderView.Text = string.Empty;
            HeaderView.TextColor = UIColor.White;
            HeaderView.SubTextColor = UIColor.FromWhiteAlpha(0.9f, 1.0f);

            var section = new Section { HeaderView = HeaderView };
            var section2 = new Section();

            var split = new SplitButtonElement();
            var stars = split.AddButton("Stargazers", "-");
            var forks = split.AddButton("Forks", "-");
            var collaborators = split.AddButton("Contributors", "-");
            section.Add(split);

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.WhenAnyValue(y => y.RepositoryIdentifier).Where(y => y != null))
                .Switch()
                .Subscribe(x => 
                {
                    Title = HeaderView.Text = x.Name;
                    ReloadData();
                });

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.WhenAnyValue(y => y.ContributorCount))
                .Switch()
                .Subscribe(x => collaborators.Text = x.HasValue ? x.Value.ToString() : "-");

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.WhenAnyValue(y => y.Repository))
                .Switch()
                .Subscribe(x =>
                {
                    if (x == null)
                    {
                        HeaderView.ImageUri = null;
                        HeaderView.Text = null;
                        HeaderView.SubText = null;
                        stars.Text = "-";
                        forks.Text = "-";
                    }
                    else
                    {
                        HeaderView.ImageUri = x.Owner.AvatarUrl;
                        HeaderView.Text = x.Name;
                        HeaderView.SubText = x.Description;
                        stars.Text = x.WatchersCount.ToString();
                        forks.Text = x.ForksCount.ToString();
                    }

                    ReloadData();
                });

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.WhenAnyValue(y => y.Liked))
                .Switch()
                .Subscribe(x =>
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

            _webElement = new WebElement("readme");
            _webElement.UrlRequested += (obj) => ViewModel.GoToUrlCommand.ExecuteIfCan(obj);

            ViewModel.WhenAnyValue(y => y.Readme).Where(_ => !_disposed).Subscribe(x =>
            {
                if (x == null)
                {
                    _webElement.ContentPath = null;
                    section2.HeaderView = new LoadingView();
                    if (_webElement.GetRootElement() != null)
                        section2.Remove(_webElement);
                }
                else
                {
                    var view = new ReadmeRazorView { Model = x };
                    var file = System.IO.Path.GetTempFileName() + ".html";
                    using (var stream = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8))
                    {
                        view.Generate(stream);
                        _webElement.ContentPath = file;

                    }

                    section2.HeaderView = null;
                    if (_webElement.GetRootElement() == null)
                        section2.Add(_webElement);
                }

                ReloadData();
            });

            ViewModel.DismissCommand.Subscribe(_ => 
            {
                _disposed = true;
                _webElement.Dispose();
            });

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
            var showCodeHub = _actionSheet.AddButton("Open in CodeHub");
            var show = _actionSheet.AddButton("Show in GitHub");
            var share = _actionSheet.AddButton("Share");
            _actionSheet.CancelButtonIndex = _actionSheet.AddButton("Cancel");
            _actionSheet.Clicked += (sender, e) =>
            {
                if (e.ButtonIndex == show)
                {
                    ViewModel.GoToGitHubCommand.ExecuteIfCan();
                }
                else if (e.ButtonIndex == showCodeHub)
                {
                    var url = new NSUrl("codehub://github.com/" + ViewModel.RepositoryIdentifier.Owner + "/" + ViewModel.RepositoryIdentifier.Name);
                    if (UIApplication.SharedApplication.CanOpenUrl(url))
                    {
                        UIApplication.SharedApplication.OpenUrl(url);
                    }
                    else
                    {
                        // Go to the CodeHub iTunes page
                        UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/codehub-github-for-ios/id707173885?mt=8"));
                    }
                }
                else if (e.ButtonIndex == share)
                {
                    var item = NSObject.FromObject(ViewModel.Repository.HtmlUrl);
                    var activityItems = new [] { item };
                    UIActivity[] applicationActivities = null;
                    var activityController = new UIActivityViewController(activityItems, applicationActivities);
                    PresentViewController(activityController, true, null);
                }

                _actionSheet = null;
            };

            _actionSheet.ShowFrom(NavigationItem.RightBarButtonItem, true);
		}

        private class LoadingView : UIView
        {
            private readonly UIActivityIndicatorView _activity;

            public LoadingView()
                : base(new RectangleF(0, 0, 320f, 44f))
            {
                BackgroundColor = UIColor.Clear;

                _activity = new UIActivityIndicatorView();
                _activity.Color = UINavigationBar.Appearance.BackgroundColor;
                _activity.Center = new PointF(Bounds.Width / 2, Bounds.Height / 2);
                _activity.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin |
                                             UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin;
                _activity.StartAnimating();
                Add(_activity);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            HeaderView.BackgroundColor = NavigationController.NavigationBar.BackgroundColor;

            if (TableView.Subviews.Length > 0)
                TableView.Subviews[0].BackgroundColor = NavigationController.NavigationBar.BackgroundColor;
        }
    }
}

