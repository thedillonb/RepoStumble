
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

        public ShowcaseTableViewCell(IntPtr handle) : base(handle)
        {
        }

        public override NSString ReuseIdentifier { get { return Key; } }

        public static ShowcaseTableViewCell Create()
        {
            var cell = (ShowcaseTableViewCell)Nib.Instantiate(null, null)[0];
            cell.ShowcaseImageView.Layer.MasksToBounds = true;
            cell.ShowcaseImageView.Layer.CornerRadius = cell.ShowcaseImageView.Frame.Height / 2f;
            cell.SeparatorInset = new UIEdgeInsets(0, cell.ShowcaseNameLabel.Frame.Left, 0, 0);
            return cell;
        }
  
        public void SetImage(string url)
        {
            if (url == null)
                ShowcaseImageView.Image = null;
                
            try
            {
                ShowcaseImageView.SetImage(new NSUrl(url));
            }
            catch {}
        }

        public string Name
        {
            get { return ShowcaseNameLabel.Text; }
            set { ShowcaseNameLabel.Text = value; }
        }

        public string Description
        {
            get { return ShowcaseDescriptionLabel.Text; }
            set { ShowcaseDescriptionLabel.Text = value; }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ContentView.SetNeedsLayout();
            ContentView.LayoutIfNeeded();

            ShowcaseDescriptionLabel.PreferredMaxLayoutWidth = ShowcaseDescriptionLabel.Frame.Width;
        }
    }
}

