using System;
using RepositoryStumble.Core.ViewModels.Trending;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;
using ReactiveUI;
using System.Linq;
using RepositoryStumble.Elements;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Reactive.Linq;
using System.Drawing;

namespace RepositoryStumble.ViewControllers.Trending
{
    public class TrendingViewController : ViewModelCollectionViewController<TrendingViewModel>
    {
        public TrendingViewController()
            : base(true, false)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.ShadowImage = new UIImage();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NavigationController.NavigationBar.ShadowImage = null;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var titleButton = new TitleButton { Frame = new System.Drawing.RectangleF(0, 0, 320f, 44f) };
            titleButton.TouchUpInside += (sender, e) => ViewModel.GoToLanguages.ExecuteIfCan();
            ViewModel.WhenAnyValue(x => x.SelectedLanguage).Subscribe(x => titleButton.Text = x.Name);
            NavigationItem.TitleView = titleButton;

            ViewModel.LoadCommand.IsExecuting.Where(x => x).Subscribe(_ =>
                TableView.ScrollRectToVisible(new System.Drawing.RectangleF(0, 0, 1, 1), true));

            ViewModel.Repositories.Changed.Subscribe(_ =>
            {
                var sections = new List<Section>();
                var repoGroups = ViewModel.Repositories.GroupBy(ViewModel.Repositories.GroupFunc);
                foreach (var g in repoGroups)
                {
                    var sec = new Section(CreateHeaderView(g.Key.ToString()));
                    foreach (var x in g.Select(x => x.Repository))
                        sec.Add(new RepositoryElement(x.Owner, x.Name, x.Description, x.AvatarUrl, () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x)));
                    sections.Add(sec);
                }
                Root.Reset(sections);
            });
        }

        private static UILabel CreateHeaderView(string name)
        {
            var v = new UILabel(new RectangleF(0, 0, 320f, 26f)) { BackgroundColor = UINavigationBar.Appearance.BarTintColor };
            v.Text = name;
            v.Font = UIFont.BoldSystemFontOfSize(14f);
            v.TextColor = UINavigationBar.Appearance.TintColor;
            v.TextAlignment = UITextAlignment.Center;
            return v;
        }

        private class TitleButton : UIButton
        {
            private readonly UILabel _label;
            private readonly UIImageView _imageView;

            public string Text
            {
                get { return _label.Text; }
                set 
                { 
                    _label.Text = value;
                    SetNeedsLayout();
                }
            }

            public TitleButton()
            {
                _label = new UILabel();
                _label.TextColor = UINavigationBar.Appearance.TintColor;
                _label.TextAlignment = UITextAlignment.Center;
                Add(_label);

                _imageView = new UIImageView();
                _imageView.Frame = new System.Drawing.RectangleF(0, 0, 12, 12);
                _imageView.TintColor = UINavigationBar.Appearance.TintColor;
                _imageView.Image = Images.DownChevron.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Add(_imageView);
            }

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                _label.SizeToFit();
                _label.Center = new System.Drawing.PointF(Frame.Width / 2f, Frame.Height / 2f);
                _imageView.Center = new System.Drawing.PointF(_label.Frame.Right + 12f, Frame.Height / 2f);
            }
        }
    }
}

