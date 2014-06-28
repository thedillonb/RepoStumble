using System;
using MonoTouch.UIKit;
using RepositoryStumble.Core.Data;

namespace RepositoryStumble.ViewControllers
{
	public class SeenStumbleViewController : RepositoryViewController
    {
		private readonly StumbledRepository _stumbledRepository;

		public SeenStumbleViewController(StumbledRepository stumbledRepository)
        {
			_stumbledRepository = stumbledRepository;

			ToolbarItems = new [] { 
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				_dislikeButton,
				new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 80 },
				_likeButton,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
			};

			if (_stumbledRepository.Liked != null)
			{
				if (_stumbledRepository.Liked.Value)
				{
					_likeButton.TintColor = SelectedColor;
					_dislikeButton.TintColor = DeselectedColor;
				}
				else
				{
					_likeButton.TintColor = DeselectedColor;
					_dislikeButton.TintColor = SelectedColor;
				}
			}
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Load(_stumbledRepository);
		}

		public override void ViewWillDisappear(bool animated)
		{
			var view = NavigationController.VisibleViewController;
			if (!(view is ProfileViewController))
				NavigationController.SetToolbarHidden(true, animated);
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
    }
}

