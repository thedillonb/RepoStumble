using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Dialog;
using MonoTouch.CoreGraphics;

namespace RepositoryStumble.Views
{
    public class HeaderView : UIView
    {
        private const float XPad = 14f;
        private const float YPad = 10f;

        public static UIFont TitleFont = UIFont.BoldSystemFontOfSize(16);
        public static UIFont SubtitleFont = UIFont.SystemFontOfSize(13);

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public UIImage Image { get; set; }

        public HeaderView()
			: base(new RectangleF(0, 0, 0, 60f))
        {
            BackgroundColor = UIColor.Clear;
            Layer.MasksToBounds = false;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOpacity = 0.4f;
            Layer.ShadowOffset = new SizeF(0, 1f);
        }

        public override void Draw(RectangleF rect)
        {
            base.Draw(rect);

            var context = UIGraphics.GetCurrentContext();
            float titleY = string.IsNullOrWhiteSpace(Subtitle) ? rect.Height / 2 - TitleFont.LineHeight / 2 : YPad;
            float contentWidth = rect.Width - XPad * 2;
            var midx = rect.Width/2;

			UIColor.White.SetColor ();
            context.FillRect (rect);

            if (Image != null)
            {
                var height = Image.Size.Height > 36 ? 36 : Image.Size.Height;
                var width = Image.Size.Width > 36 ? 36 : Image.Size.Width;
                var top = rect.Height / 2 - height / 2;
                var left = rect.Width - XPad - width;

                Image.Draw(new RectangleF(left, top, width, height));
                contentWidth -= (width + XPad * 2); 
            }


            if (!string.IsNullOrEmpty(Title))
            {
				UIColor.Black.SetColor();
                DrawString(
                        Title,
                        new RectangleF(XPad, titleY, contentWidth, TitleFont.LineHeight),
                        TitleFont,
                        UILineBreakMode.TailTruncation
                );
            }

            if (!string.IsNullOrWhiteSpace(Subtitle))
            {
				UIColor.DarkGray.SetColor();
                DrawString(
                    Subtitle,
                    new RectangleF(XPad, YPad + TitleFont.LineHeight + 2f, contentWidth, SubtitleFont.LineHeight),
                    SubtitleFont,
                    UILineBreakMode.TailTruncation
                );
            }


        }
    }
}

