// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RepositoryStumble.TableViewCells
{
	[Register ("RepositoryTableViewCell")]
	partial class RepositoryTableViewCell
	{
		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel OwnerLabel { get; set; }

		[Outlet]
		UIKit.UIImageView RepositoryImageView { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (OwnerLabel != null) {
				OwnerLabel.Dispose ();
				OwnerLabel = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (RepositoryImageView != null) {
				RepositoryImageView.Dispose ();
				RepositoryImageView = null;
			}
		}
	}
}
