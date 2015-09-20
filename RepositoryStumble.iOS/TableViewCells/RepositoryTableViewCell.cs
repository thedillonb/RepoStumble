using System;
using Foundation;
using UIKit;
using SDWebImage;

namespace RepositoryStumble.TableViewCells
{
    public partial class RepositoryTableViewCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("RepositoryTableViewCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("RepositoryTableViewCell");

        public RepositoryTableViewCell(IntPtr handle) 
            : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            RepositoryImageView.Layer.MasksToBounds = true;
            RepositoryImageView.Layer.CornerRadius = RepositoryImageView.Frame.Height / 2f;
        }

        public void Set(string name, string owner, string description, string avatarUrl)
        {
            TitleLabel.Text = name;
            OwnerLabel.Text = owner;
            DescriptionLabel.Text = description;

            if (avatarUrl == null)
                RepositoryImageView.Image = null;
            else
            {
                try
                {
                    RepositoryImageView.SetImage(new NSUrl(avatarUrl), Images.UnknownUser, (img, err, type, imageUrl) => {
                        if (img == null || err != null)
                            return;

                        if (type == SDImageCacheType.None)
                        {
                            RepositoryImageView.Image = Images.UnknownUser;
                            BeginInvokeOnMainThread(() =>
                                UIView.Transition(RepositoryImageView, 0.25f, UIViewAnimationOptions.TransitionCrossDissolve, () => RepositoryImageView.Image = img, null));
                        }
                    });
                }
                catch
                {
                }
            }
        }
    }
}

