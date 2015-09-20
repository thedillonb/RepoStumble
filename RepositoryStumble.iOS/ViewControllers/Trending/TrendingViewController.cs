using System;
using RepositoryStumble.Core.ViewModels.Trending;
using Xamarin.Utilities.DialogElements;
using ReactiveUI;
using System.Linq;
using RepositoryStumble.Elements;
using System.Collections.Generic;
using UIKit;
using System.Reactive.Linq;
using CoreGraphics;
using RepositoryStumble.TableViewCells;

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

            var titleButton = new TitleButton { Frame = new CGRect(0, 0, 320f, 44f) };
            titleButton.TouchUpInside += (sender, e) => ViewModel.GoToLanguages.ExecuteIfCan();
            ViewModel.WhenAnyValue(x => x.SelectedLanguage).Subscribe(x => titleButton.Text = x.Name);
            NavigationItem.TitleView = titleButton;

            ViewModel.LoadCommand.IsExecuting.Where(x => x).Subscribe(_ =>
                TableView.ScrollRectToVisible(new CGRect(0, 0, 1, 1), true));

            TableView.RegisterNibForCellReuse(RepositoryTableViewCell.Nib, RepositoryTableViewCell.Key);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 80f;

            ViewModel.Repositories.Changed.Subscribe(_ =>
            {
                var sections = new List<Section>();
                var repoGroups = ViewModel.Repositories.GroupBy(x => x.Time);
                foreach (var g in repoGroups)
                {
                    var sec = new Section(CreateHeaderView(g.Key));
                    foreach (var x in g.Select(x => x.Repository).Where(x => x.Owner != null))
                        sec.Add(new RepositoryElement(x.Owner.Login, x.Name, x.Description, x.Owner.AvatarUrl, () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x)));
                    sections.Add(sec);
                }
                Root.Reset(sections);
            });
        }

        private static UILabel CreateHeaderView(string name)
        {
            var v = new UILabel(new CGRect(0, 0, 320f, 26f)) { BackgroundColor = UINavigationBar.Appearance.BarTintColor };
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
                _imageView.Frame = new CGRect(0, 0, 12, 12);
                _imageView.TintColor = UINavigationBar.Appearance.TintColor;
                _imageView.Image = Images.DownChevron.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Add(_imageView);
            }

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                _label.SizeToFit();
                _label.Center = new CGPoint(Frame.Width / 2f, Frame.Height / 2f);
                _imageView.Center = new CGPoint(_label.Frame.Right + 12f, Frame.Height / 2f);
            }
        }
    }
}

