using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace RepositoryStumble.TableViewCells
{
    public partial class RepositoryTableViewCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("RepositoryTableViewCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("RepositoryTableViewCell");

        public RepositoryTableViewCell(IntPtr handle) : base(handle)
        {
        }

        public static RepositoryTableViewCell Create()
        {
            var cell = (RepositoryTableViewCell)Nib.Instantiate(null, null)[0];
            cell.RepositoryImageView.Layer.MasksToBounds = true;
            cell.RepositoryImageView.Layer.CornerRadius = cell.RepositoryImageView.Frame.Height / 2f;
            cell.SeparatorInset = new UIEdgeInsets(0, cell.TitleLabel.Frame.Left, 0, 0);
            return cell;
        }

        public UIImage Image 
        {
            get { return RepositoryImageView.Image; }
            set { RepositoryImageView.Image = value; }
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

