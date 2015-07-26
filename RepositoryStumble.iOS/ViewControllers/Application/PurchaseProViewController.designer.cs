// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RepositoryStumble.ViewControllers.Application
{
	[Register ("PurchaseProViewController")]
	partial class PurchaseProViewController
	{
		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UIImageView MainImageView { get; set; }

		[Outlet]
		UIKit.UIButton PurchaseButton { get; set; }

		[Outlet]
		UIKit.UIButton RestoreButton { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
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
