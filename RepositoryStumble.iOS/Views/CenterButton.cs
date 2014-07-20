using MonoTouch.UIKit;

namespace RepositoryStumble.Views
{
	public class CenterButton : UIButton
	{
        private readonly UIImageView _imageView;

        public bool Disabled
        {
            set
            {
                _imageView.Image = value ? Images.CenterSearchDisabled : Images.CenterSearch;
            }
        }

        public CenterButton()
			: base(UIButtonType.Custom)
        {
			this.Layer.MasksToBounds = false;

            _imageView = new UIImageView(Images.CenterSearch);
            _imageView.UserInteractionEnabled = false;

            this.Frame = new System.Drawing.RectangleF(0, 0, _imageView.Frame.Width, 44f);
            _imageView.Frame = new System.Drawing.RectangleF(0, 44f - _imageView.Frame.Height, _imageView.Frame.Width, _imageView.Frame.Height);

            Add(_imageView);
        }
    }
}

