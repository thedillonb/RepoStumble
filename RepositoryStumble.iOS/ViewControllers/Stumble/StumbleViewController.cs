using System;
using MonoTouch.UIKit;
using RepositoryStumble.Core.ViewModels.Stumble;
using RepositoryStumble.Views;
using RepositoryStumble.ViewControllers.Repositories;
using ReactiveUI;
using Xamarin.Utilities.Core.Services;
using BigTed;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace RepositoryStumble.ViewControllers.Stumble
{
	public class StumbleViewController : BaseRepositoryViewController<StumbleViewModel>
    {
        private Hud _hud;

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

            ViewModel.StumbleCommand.CanExecuteObservable.Subscribe(x => centerButton.Disabled = !x);
            DislikeButton.EnableIfExecutable(ViewModel.StumbleCommand.CanExecuteObservable);
            LikeButton.EnableIfExecutable(ViewModel.StumbleCommand.CanExecuteObservable);

            ViewModel.DislikeCommand.Subscribe(_ => DisplayResponse(Images.ThumbDownFilled));
            ViewModel.LikeCommand.Subscribe(_ => DisplayResponse(Images.ThumbUpFilled));

            ViewModel.StumbleCommand.ExecuteIfCan();
		}

        private void DisplayResponse(UIImage img)
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var initialBounds = window.Bounds;
            initialBounds.Inflate(new SizeF(window.Bounds.Width, window.Bounds.Height));

            var hud = _hud = new Hud(initialBounds, img);
            window.Add(hud);
            hud.Center = new PointF(window.Bounds.Width / 2f, window.Bounds.Height / 2f);
            hud.BackgroundColor = UINavigationBar.Appearance.BackgroundColor;
            hud.Alpha = 0f;

            UIView.Animate(0.3f, 0, UIViewAnimationOptions.CurveEaseOut, () => 
                {
                    hud.Frame = window.Bounds;
                    hud.Alpha = 1f;
                }, 
                () => UIView.Animate(0.7f, 0.4f, UIViewAnimationOptions.CurveEaseIn, () =>
                {
                    hud.Alpha = 0f;
                    hud.Frame = initialBounds;
                }, hud.RemoveFromSuperview)
            );
        }

        private class Hud : UIView
        {
            private readonly UIImageView _imageView;

            public Hud(RectangleF frame, UIImage image)
                : base(frame)
            {
                UserInteractionEnabled = true;

                _imageView = new UIImageView(new RectangleF(0, 0, frame.Width / 5f, frame.Width / 5f));
                _imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                _imageView.Center = new PointF(Frame.Width / 2f, Frame.Height / 2f);
                _imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleMargins;
                _imageView.TintColor = UIColor.White;
                _imageView.Image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                Add(_imageView);
            }
        }
    }
}

