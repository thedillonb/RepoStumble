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
	[Register ("ShowcaseTableViewCell")]
	partial class ShowcaseTableViewCell
	{
		[Outlet]
		UIKit.UILabel ShowcaseDescriptionLabel { get; set; }

		[Outlet]
		UIKit.UIImageView ShowcaseImageView { get; set; }

		[Outlet]
		UIKit.UILabel ShowcaseNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ShowcaseDescriptionLabel != null) {
				ShowcaseDescriptionLabel.Dispose ();
				ShowcaseDescriptionLabel = null;
			}

			if (ShowcaseImageView != null) {
				ShowcaseImageView.Dispose ();
				ShowcaseImageView = null;
			}

			if (ShowcaseNameLabel != null) {
				ShowcaseNameLabel.Dispose ();
				ShowcaseNameLabel = null;
			}
		}
	}
}
