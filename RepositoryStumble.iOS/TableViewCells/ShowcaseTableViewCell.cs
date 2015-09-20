using System;
using Foundation;
using UIKit;
using SDWebImage;

namespace RepositoryStumble.TableViewCells
{
    public partial class ShowcaseTableViewCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("ShowcaseTableViewCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("ShowcaseTableViewCell");

        public ShowcaseTableViewCell(IntPtr handle) 
            : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            ShowcaseImageView.Layer.MasksToBounds = true;
            ShowcaseImageView.Layer.CornerRadius = ShowcaseImageView.Frame.Height / 2f;
        }

        public void Set(string name, string description, string avatarUrl)
        {
            ShowcaseNameLabel.Text = name;
            ShowcaseDescriptionLabel.Text = description;

            if (avatarUrl == null)
                ShowcaseImageView.Image = null;
            else
            {
                try
                {
                    ShowcaseImageView.SetImage(new NSUrl(avatarUrl), Images.UnknownUser, (img, err, type, imageUrl) => {
                        if (img == null || err != null)
                            return;

                        if (type == SDImageCacheType.None)
                        {
                            ShowcaseImageView.Image = Images.UnknownUser;
                            BeginInvokeOnMainThread(() =>
                                UIView.Transition(ShowcaseImageView, 0.25f, UIViewAnimationOptions.TransitionCrossDissolve, () => ShowcaseImageView.Image = img, null));
                        }
                    });
                }
                catch {}
            }
        }
    }
}

