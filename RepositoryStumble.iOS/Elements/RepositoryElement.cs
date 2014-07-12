using System;
using Xamarin.Utilities.DialogElements;
using Xamarin.Utilities.Images;
using RepositoryStumble.TableViewCells;
using ReactiveUI.Cocoa;
using System.Drawing;
using MonoTouch.UIKit;

namespace RepositoryStumble.Elements
{
    public class RepositoryElement : Element, IImageUpdated, IElementSizing
    {
        private string _name;
        private string _owner;
        private string _description;
        private string _imageUrl;
        private Action _tapped;

        public RepositoryElement(string owner, string name, string description, string imageUrl, Action tapped = null)
        {
            _tapped = tapped;
            _owner = owner;
            _name = name;
            _description = description;
            _imageUrl = imageUrl;
        }

        public override void Selected(UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
        {
            if (_tapped != null)
                _tapped();
            base.Selected(tableView, path);
        }

        public void UpdatedImage(Uri uri)
        {
            var img = ImageLoader.DefaultRequestImage(uri, this);
            if (img != null)
            {
                var cell = GetActiveCell() as RepositoryTableViewCell;
                if (cell != null)
                {
                    cell.Image = img;
                    cell.SetNeedsDisplay();
                }
                //GetRootElement().Reload(this, UITableViewRowAnimation.None);
            }
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(RepositoryTableViewCell.Key) as RepositoryTableViewCell;
            if (cell == null)
            {
                cell = RepositoryTableViewCell.Create();
            }

            cell.Image = ImageLoader.DefaultRequestImage(new Uri(_imageUrl), this);
            cell.Owner = _owner;
            cell.Description = _description;
            cell.Name = _name;
            return cell;
        }

        public float GetHeight(MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(RepositoryTableViewCell.Key) as RepositoryTableViewCell;
            if (cell == null)
            {
                cell = RepositoryTableViewCell.Create();
            }

            cell.Owner = _owner;
            cell.Description = _description;
            cell.Name = _name;

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            cell.Bounds = new RectangleF(0, 0, tableView.Bounds.Width, tableView.Bounds.Height);

            cell.SetNeedsLayout();
            cell.LayoutIfNeeded();

            return cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height + 1;
        }
    }
}

