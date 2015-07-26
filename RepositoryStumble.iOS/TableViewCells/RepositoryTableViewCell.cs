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

        public override NSString ReuseIdentifier { get { return Key; } }

        public static RepositoryTableViewCell Create()
        {
            var cell = (RepositoryTableViewCell)Nib.Instantiate(null, null)[0];
            cell.RepositoryImageView.Layer.MasksToBounds = true;
            cell.RepositoryImageView.Layer.CornerRadius = cell.RepositoryImageView.Frame.Height / 2f;
            cell.SeparatorInset = new UIEdgeInsets(0, cell.TitleLabel.Frame.Left, 0, 0);
            return cell;
        }

        public void SetImage(string url, UIImage placeholder)
        {
            if (url == null)
            {
                RepositoryImageView.Image = null;
                return;
            }
            
            try
            {
                RepositoryImageView.SetImage(new NSUrl(url), placeholder);
            }
            catch
            {
            }
        }

        public void SetImage(UIImage placeholder)
        {
            RepositoryImageView.Image = placeholder;
        }

        public string Name
        {
            get { return TitleLabel.Text; }
            set { TitleLabel.Text = value; }
        }

        public string Description
        {
            get { return DescriptionLabel.Text; }
            set { DescriptionLabel.Text = value; }
        }

        public string Owner
        {
            get { return OwnerLabel.Text; }
            set { OwnerLabel.Text = value; }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ContentView.SetNeedsLayout();
            ContentView.LayoutIfNeeded();

            DescriptionLabel.PreferredMaxLayoutWidth = DescriptionLabel.Frame.Width;
        }
    }
}

