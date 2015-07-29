using System;
using System.Reactive.Linq;
using UIKit;
using RepositoryStumble.Core.ViewModels;
using RepositoryStumble.Views;

namespace RepositoryStumble.ViewControllers
{
    public abstract class ViewModelPrettyDialogViewController
    {
        public static UIColor RefreshIndicatorColor = UIColor.LightGray;
    }

    public abstract class ViewModelPrettyDialogViewController<TViewModel> : ViewModelDialogViewController<TViewModel> where TViewModel : class, IBaseViewModel
    {
        protected SlideUpTitleView SlideUpTitle;

        protected ImageAndTitleHeaderView HeaderView;

        public override string Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                if (HeaderView != null) HeaderView.Text = value;
                if (SlideUpTitle != null) SlideUpTitle.Text = value;
                base.Title = value;
            }
        }

        protected ViewModelPrettyDialogViewController()
            : base(true)
        {
            Scrolled.Where(x => x.Y > 0)
                .Where(_ => NavigationController != null)
                .Subscribe(_ => NavigationController.NavigationBar.ShadowImage = null);
            Scrolled.Where(x => x.Y <= 0)
                .Where(_ => NavigationController != null)
                .Where(_ => NavigationController.NavigationBar.ShadowImage == null)
                .Subscribe(_ => NavigationController.NavigationBar.ShadowImage = new UIImage());
            Scrolled.Where(_ => SlideUpTitle != null).Subscribe(x => SlideUpTitle.Offset = 108 + 28f - x.Y);
        }

        public override void ViewWillAppear(bool animated)
        {
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(false, animated);
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.ShadowImage = new UIImage();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NavigationController.NavigationBar.ShadowImage = null;
            if (ToolbarItems != null)
                NavigationController.SetToolbarHidden(true, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.TitleView = SlideUpTitle = new SlideUpTitleView(NavigationController.NavigationBar.Bounds.Height) { Text = Title };
            SlideUpTitle.Offset = 100f;

            TableView.SectionHeaderHeight = 0;

            if (RefreshControl != null)
                RefreshControl.TintColor = ViewModelPrettyDialogViewController.RefreshIndicatorColor;

            HeaderView = new ImageAndTitleHeaderView 
            { 
                BackgroundColor = NavigationController.NavigationBar.BackgroundColor,
                TextColor = UIColor.White,
                SubTextColor = UIColor.LightGray,
                Text = base.Title
            };

            TableView.CreateTopBackground(HeaderView.BackgroundColor);

            //            this.WhenAnyValue(x => x.ViewModel)
            //                .Subscribe(x =>
            //                {
            //
            //                });
            //
            //            var loadableViewModel = ViewModel as ILoadableViewModel;
            //            if (loadableViewModel != null)
            //            {
            //                topBackgroundView.Hidden = true;
            //                loadableViewModel.LoadCommand.IsExecuting.Where(x => !x).Skip(1).Take(1).Subscribe(_ => topBackgroundView.Hidden = false);
            //            }
        }
    }

    public static class ViewControllerExtensions
    {

        public static UIView CreateTopBackground(this UIView view, UIColor color)
        {
            var frame = view.Bounds;
            frame.Y = -frame.Size.Height;
            var view2 = new UIView(frame);
            view2.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            view2.BackgroundColor = color;
            view.InsertSubview(view2, 0);
            return view;
        }
    }
}
