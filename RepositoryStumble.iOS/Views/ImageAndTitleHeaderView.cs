using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog.Utilities;
using System.Drawing;

namespace RepositoryStumble.Views
{
    public class ImageAndTitleHeaderView : UIView, IImageUpdated
    {
        private readonly UIImageView _imageView;
        private readonly UILabel _label;
        private readonly UILabel _label2;
        private readonly UIView _seperatorView;

        public string ImageUri
        {
            set
            {
                if (value == null)
                    _imageView.Image = null;
                else
                {
                    var img = ImageLoader.DefaultRequestImage(new Uri(value), this);
                    if (img != null)
                        UIView.Transition(_imageView, 0.25f, UIViewAnimationOptions.TransitionCrossDissolve, () => _imageView.Image = img, null);
                }
            }
        }

        public UIImage Image
        {
            get { return _imageView.Image; }
            set { _imageView.Image = value; }
        }

        public string Text
        {
            get { return _label.Text; }
            set 
            { 
                _label.Text = value; 
                this.SetNeedsLayout();
                this.LayoutIfNeeded();
            }
        }

        public UIColor TextColor
        {
            get { return _label.TextColor; }
            set
            {
                _label.TextColor = value;
            }
        }

        public string SubText
        {
            get { return _label2.Text; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _label2.Hidden = false;
                _label2.Text = value;
                this.SetNeedsLayout();
                this.LayoutIfNeeded();
            }
        }

        public UIColor SubTextColor
        {
            get { return _label2.TextColor; }
            set
            {
                _label2.TextColor = value;
            }
        }

        public bool EnableSeperator
        {
            get
            {
                return !_seperatorView.Hidden;
            }
            set
            {
                _seperatorView.Hidden = !value;
            }
        }

        public UIColor SeperatorColor
        {
            get
            {
                return _seperatorView.BackgroundColor;
            }
            set
            {
                _seperatorView.BackgroundColor = value;
            }
        }

        public bool RoundedImage
        {
            get { return _imageView.Layer.CornerRadius > 0; }
            set
            {
                if (value)
                {
                    _imageView.Layer.CornerRadius = _imageView.Frame.Width / 2f;
                    _imageView.Layer.MasksToBounds = true;
                }
                else
                {
                    _imageView.Layer.MasksToBounds = false;
                    _imageView.Layer.CornerRadius = 0;
                }
            }
        }

        public UIColor ImageTint
        {
            get { return _imageView.TintColor; }
            set { _imageView.TintColor = value; }
        }

        public ImageAndTitleHeaderView()
            : base(new RectangleF(0, 0, 320f, 100f))
        {
            _imageView = new UIImageView();
            _imageView.Frame = new RectangleF(0, 0, 80, 80);
            _imageView.BackgroundColor = UIColor.White;
            _imageView.Layer.BorderWidth = 2f;
            _imageView.Layer.BorderColor = UIColor.White.CGColor;
            Add(_imageView);

            _label = new UILabel();
            _label.TextAlignment = UITextAlignment.Center;
            _label.Lines = 0;
            Add(_label);

            _label2 = new UILabel();
            _label2.Hidden = true;
            _label2.TextAlignment = UITextAlignment.Center;
            _label2.Font = UIFont.SystemFontOfSize(14f);
            _label2.Lines = 1;
            Add(_label2);

            _seperatorView = new UIView();
            _seperatorView.BackgroundColor = UIColor.FromWhiteAlpha(214.0f / 255.0f, 1.0f);
            Add(_seperatorView);

            EnableSeperator = false;
            RoundedImage = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _imageView.Center = new PointF(Bounds.Width / 2, 15 + _imageView.Frame.Height / 2);

            _label.Frame = new RectangleF(20, _imageView.Frame.Bottom + 10f, Bounds.Width - 40, Bounds.Height - (_imageView.Frame.Bottom + 5f));
            _label.SizeToFit();
            _label.Frame = new RectangleF(20, _imageView.Frame.Bottom + 10f, Bounds.Width - 40, _label.Frame.Height);

            _label2.Frame = new RectangleF(20, _label.Frame.Bottom + 2f, Bounds.Width - 40f, 16f);
            _label2.SizeToFit();
            _label2.Frame = new RectangleF(20, _label.Frame.Bottom + 2f, Bounds.Width - 40f, _label2.Frame.Height);

            var bottom = _label2.Hidden == false? _label2.Frame.Bottom : _label.Frame.Bottom;
            var f = Frame;
            f.Height = bottom + 15f;
            Frame = f;

            _seperatorView.Frame = new RectangleF(0, Frame.Height - 0.5f, Frame.Width, 0.5f);
        }

        public void UpdatedImage(Uri uri)
        {
            var img = ImageLoader.DefaultRequestImage(uri, this);
            UIView.Transition(_imageView, 0.25f, UIViewAnimationOptions.TransitionCrossDissolve, () => _imageView.Image = img, null);
        }
    }
}

