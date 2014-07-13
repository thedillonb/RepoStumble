
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace RepositoryStumble.TableViewCells
{
    public partial class ShowcaseTableViewCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("ShowcaseTableViewCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("ShowcaseTableViewCell");

        public ShowcaseTableViewCell(IntPtr handle) : base(handle)
        {
        }

        public static ShowcaseTableViewCell Create()
        {
            var cell = (ShowcaseTableViewCell)Nib.Instantiate(null, null)[0];
            cell.ShowcaseImageView.Layer.MasksToBounds = true;
            cell.ShowcaseImageView.Layer.CornerRadius = cell.ShowcaseImageView.Frame.Height / 2f;
            cell.SeparatorInset = new UIEdgeInsets(0, cell.ShowcaseNameLabel.Frame.Left, 0, 0);
            return cell;

        }
  
        public UIImage Image 
        {
            get { return ShowcaseImageView.Image; }
            set { ShowcaseImageView.Image = value; }
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

