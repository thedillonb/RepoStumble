// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace RepositoryStumble.ViewControllers.Application
{
	[Register ("PurchaseProViewController")]
	partial class PurchaseProViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView MainImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton PurchaseButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton RestoreButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (MainImageView != null) {
				MainImageView.Dispose ();
				MainImageView = null;
			}

			if (PurchaseButton != null) {
				PurchaseButton.Dispose ();
				PurchaseButton = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (RestoreButton != null) {
				RestoreButton.Dispose ();
				RestoreButton = null;
			}
		}
	}
}
