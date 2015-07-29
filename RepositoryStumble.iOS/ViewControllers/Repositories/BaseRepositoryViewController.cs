using System;
using UIKit;
using RepositoryStumble.Core.ViewModels.Repositories;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;
using System.Reactive.Linq;
using RepositoryStumble.Views;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public abstract class BaseRepositoryViewController<TViewModel> : ViewModelViewController<TViewModel> where TViewModel : BaseRepositoryViewModel
	{
        private UIWebView _web;
        private UIActionSheet _actionSheet;
		protected readonly UIBarButtonItem DislikeButton;
		protected readonly UIBarButtonItem LikeButton;

        private SlideUpTitleView _slideUpTitle;

        public override string Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                if (_slideUpTitle != null) _slideUpTitle.Text = value;
                base.Title = value;
            }
        }

		protected BaseRepositoryViewController()
		{
            DislikeButton = new UIBarButtonItem(Images.ThumbDown, UIBarButtonItemStyle.Plain, (s, e) => ViewModel.DislikeCommand.ExecuteIfCan());
            DislikeButton.TintColor = UITabBar.Appearance.TintColor;

            LikeButton = new UIBarButtonItem(Images.ThumbUp, UIBarButtonItemStyle.Plain, (s, e) => ViewModel.LikeCommand.ExecuteIfCan());
            LikeButton.TintColor = UITabBar.Appearance.TintColor;
        }

        private bool _loaded;
        private bool IsWebLoaded
        {
            get { return _loaded; }
            set { this.RaiseAndSetIfChanged(ref _loaded, value); }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.TitleView = _slideUpTitle = new SlideUpTitleView(NavigationController.NavigationBar.Bounds.Height) { Text = Title };
            _slideUpTitle.Offset = 100f;

            _web = new UIWebView(new CGRect(0, 0, View.Frame.Width, View.Frame.Height));
            _web.BackgroundColor = UIColor.White;
            _web.Opaque = false;
            _web.AutoresizingMask = UIViewAutoresizing.All;
            _web.LoadFinished += (sender, e) => IsWebLoaded = true;
            _web.ShouldStartLoad = (w, r, x) =>
            {
                if (x == UIWebViewNavigationType.LinkClicked && r.Url.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    ViewModel.GoToUrlCommand.ExecuteIfCan(r.Url.AbsoluteString);
                    return false;
                }
                return true;
            };
            _web.ScrollView.Scrolled += WebScrolled;
            Add(_web);

            _web.ScrollView.CreateTopBackground(UIColor.FromRGB(0x4e, 0x4b, 0xbe));

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, (s, e) => ShowMore());
            NavigationItem.RightBarButtonItem.EnableIfExecutable(this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.WhenAnyValue(y => y.Repository))
                .Switch().Select(x => x != null));

            this.WhenAnyObservable(x => x.ViewModel.LoadCommand.IsExecuting)
                .Where(x => x)
                .Subscribe(_ => IsWebLoaded = false);

            this.WhenAnyValue(x => x.ViewModel.Repository)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    _web.LoadHtmlString(new ReadmeRazorView().GenerateString(), new NSUrl(x.HtmlUrl + "/raw/" + x.DefaultBranch + "/")); 
                    Title = x.Name;
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
            
            this.WhenAnyValue(y => y.ViewModel.Repository, y => y.IsWebLoaded)
                .Where(y => y.Item1 != null && y.Item2)
                .Subscribe(x =>
                {
                    var s = JsonConvert.SerializeObject(x.Item1);
                    _web.EvaluateJavascript("setRepository(" + s + ")");
                });

            this.WhenAnyValue(y => y.ViewModel.ContributorCount, y => y.IsWebLoaded)
                .Where(y => y.Item1 != null && y.Item2)
                .Subscribe(x =>
                {
                    _web.EvaluateJavascript("setContrib(" + x.Item1.Value + ")");
                });

            this.WhenAnyValue(y => y.ViewModel.Readme, y => y.IsWebLoaded)
                .Where(y => y.Item2 && y.Item1 != null)
                .Subscribe(x =>
                {
                    var s = JsonConvert.SerializeObject(x.Item1);
                    _web.EvaluateJavascript("setBody(" + s + ")");
                });

            ToolbarItems = new [] 
            { 
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                DislikeButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 80 },
                LikeButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
            };
        }

        void WebScrolled (object sender, EventArgs e)
        {
            if (NavigationController == null)
                return;

            var x = _web.ScrollView.ContentOffset;

            if (x.Y > 0)
                NavigationController.NavigationBar.ShadowImage = null;
            if (x.Y <= 0 && NavigationController.NavigationBar.ShadowImage == null)
                NavigationController.NavigationBar.ShadowImage = new UIImage();
            
            _slideUpTitle.Offset = 108 + 28f - x.Y;
        }

		private void ShowMore()
		{
            _actionSheet = new UIActionSheet();
            var showCodeHub = _actionSheet.AddButton("Open in CodeHub");
            var show = _actionSheet.AddButton("Show in GitHub");
            var showSafari = _actionSheet.AddButton("Show in Safari");
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
                else if (e.ButtonIndex == showSafari)
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(ViewModel.Repository.HtmlUrl));
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
                : base(new CGRect(0, 0, 320f, 44f))
            {
                BackgroundColor = UIColor.Clear;

                _activity = new UIActivityIndicatorView();
                _activity.Color = UINavigationBar.Appearance.BackgroundColor;
                _activity.Center = new CGPoint(Bounds.Width / 2, Bounds.Height / 2);
                _activity.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin |
                                             UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin;
                _activity.StartAnimating();
                Add(_activity);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(false, animated);
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.ShadowImage = new UIImage();
            _web.Frame = new CGRect(0, 0, View.Frame.Width, View.Frame.Height);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NavigationController.NavigationBar.ShadowImage = null;
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(true, animated);
        }

    }
}

