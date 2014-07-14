using System;
using MonoTouch.UIKit;
using RepositoryStumble.Core.ViewModels.Stumble;
using RepositoryStumble.Views;
using RepositoryStumble.ViewControllers.Repositories;
using ReactiveUI;
using System.Reactive.Linq;

namespace RepositoryStumble.ViewControllers.Stumble
{
	public class StumbleViewController : BaseRepositoryViewController<StumbleViewModel>
    {
        private UIView _loadingView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            var centerButton = new CenterButton();
            centerButton.TouchUpInside += (s, e) =>
            {
                TableView.ScrollRectToVisible(new System.Drawing.RectangleF(0, 0, 1, 1), false);
                ViewModel.StumbleCommand.ExecuteIfCan();
            };
 
            var stumbleButton = new UIBarButtonItem(centerButton) { Enabled = false };
            stumbleButton.EnableIfExecutable(ViewModel.StumbleCommand.CanExecuteObservable);

            ToolbarItems = new [] 
            { 
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                DislikeButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 40 },
                stumbleButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 40 },
                LikeButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
            };

            DislikeButton.EnableIfExecutable(ViewModel.StumbleCommand.CanExecuteObservable);
            LikeButton.EnableIfExecutable(ViewModel.StumbleCommand.CanExecuteObservable);

            _loadingView = new UIView() { BackgroundColor = UINavigationBar.Appearance.BarTintColor };
            _loadingView.UserInteractionEnabled = true;


            var finished = false;
            ViewModel.StumbleCommand.IsExecuting.Skip(1).Subscribe(x =>
            {
                if (x)
                {
                    _loadingView.Frame = TableView.Bounds;
                    _loadingView.Alpha = 0;
                    TableView.Add(_loadingView);

                    finished = false;
                    UIView.Animate(1.0f, 0f, UIViewAnimationOptions.CurveEaseInOut,
                        () => _loadingView.Alpha = 1f,
                        () => 
                    {
                        if (!ViewModel.StumbleCommand.IsExecuting.First())
                        {
                            UIView.Animate(1.0f, 0f, UIViewAnimationOptions.CurveEaseInOut,
                                () => _loadingView.Alpha = 0f,
                                () => _loadingView.RemoveFromSuperview());
                        }
                        else
                        {
                            finished = true;
                        }
                    });
                }
                else
                {

                    if (finished)
                    {
                        UIView.Animate(1.0f, 0f, UIViewAnimationOptions.CurveEaseInOut,
                            () => _loadingView.Alpha = 0f,
                            () => _loadingView.RemoveFromSuperview());
                    }
                }
            });
		}
    }
}

