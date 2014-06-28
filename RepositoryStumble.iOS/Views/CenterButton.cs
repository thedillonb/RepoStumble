using System;
using MonoTouch.UIKit;

namespace RepositoryStumble.Views
{
	public class CenterButton : UIButton
	{
        public CenterButton()
			: base(UIButtonType.Custom)
        {
			this.Layer.MasksToBounds = false;

			var imageView = new UIImageView(Images.CenterSearch);
			imageView.UserInteractionEnabled = false;

			this.Frame = new System.Drawing.RectangleF(0, 0, imageView.Frame.Width, 44f);
			imageView.Frame = new System.Drawing.RectangleF(0, 44f - imageView.Frame.Height, imageView.Frame.Width, imageView.Frame.Height);

			Add(imageView);
        }
    }
}

