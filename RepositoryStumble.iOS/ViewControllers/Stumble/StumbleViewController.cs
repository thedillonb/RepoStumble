using System;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using RepositoryStumble.Core.ViewModels.Stumble;
using RepositoryStumble.Views;
using RepositoryStumble.Core.Data;

namespace RepositoryStumble.ViewControllers.Repositories
{
	public class StumbleViewController : BaseRepositoryViewController<StumbleViewModel>
    {
		private readonly UIBarButtonItem _stumbleButton;
		private readonly Interest _interest;

		public StumbleViewController(Interest interest = null)
        {
			_interest = interest;
			var centerButton = new CenterButton();
			centerButton.TouchUpInside += (s, e) => Stumble();

            ToolbarItems = new [] { 
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				DislikeButton,
				new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 40 },
				(_stumbleButton = new UIBarButtonItem(centerButton) { Enabled = false }),
				new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 40 },
                LikeButton,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
            };

			LikeButton.TintColor = DeselectedColor;
			LikeButton.Enabled = false;
			DislikeButton.TintColor = DeselectedColor;
			DislikeButton.Enabled = false;
        }

		private async void Stumble()
		{
			_stumbleButton.Enabled = false;
			DislikeButton.Enabled = false;
			LikeButton.Enabled = false;
//			Application.StumbleResult stumbleResult = null;
//
//			try
//			{
//				MonoTouch.Utilities.PushNetworkActive();
//
//				try
//				{
//					NavigationItem.RightBarButtonItem.Enabled = false;
//					Clear();
//					stumbleResult = await Application.Instance.StumbleRepository(_interest);
//					NavigationItem.RightBarButtonItem.Enabled = true;
//				}
//				catch (InterestExhaustedException e)
//				{
//					MonoTouch.Utilities.ShowAlert("You've seen it all!", e.Message);
//					NavigationController.PopViewControllerAnimated(true);
//					return;
//				}
//				catch (Exception e)
//				{
//					MonoTouch.Utilities.ShowAlert("Error", e.Message);
//					_stumbleButton.Enabled = true;
//					return;
//				}
//
//				try
//				{
//					await Load(stumbleResult.Repository);
//				}
//				catch (Exception e)
//				{
//					MonoTouch.Utilities.ShowAlert("Error", e.Message);
//				}
//			}
//			finally
//			{
//				MonoTouch.Utilities.PopNetworkActive();
//			}

			DislikeButton.Enabled = true;
			LikeButton.Enabled = true;
			_stumbleButton.Enabled = true;
			LikeButton.TintColor = DeselectedColor;
			DislikeButton.TintColor = DeselectedColor;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Stumble();
		}

		public override void ViewWillDisappear(bool animated)
		{
//			var view = NavigationController.VisibleViewController;
//			if (!(view is ProfileViewController))
//				NavigationController.SetToolbarHidden(true, animated);
			base.ViewWillDisappear(animated);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.SetToolbarHidden(false, true);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.Toolbar.Translucent = false;
			NavigationController.Toolbar.BarTintColor = UIColor.FromRGB(245, 245, 245);
		}

		protected override void Like()
		{
			base.Like();
			Stumble();
		}

		protected override void Dislike()
		{
			base.Dislike();
			Stumble();
		}
    }
}

