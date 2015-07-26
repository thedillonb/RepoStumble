using System;
using Xamarin.Utilities.DialogElements;
using RepositoryStumble.TableViewCells;
using ReactiveUI.Cocoa;
using CoreGraphics;
using UIKit;

namespace RepositoryStumble.Elements
{
    public class ShowcaseElement : Element, IElementSizing
    {
        private string _name;
        private string _description;
        private string _imageUrl;
        private Action _tapped;

        public ShowcaseElement(string name, string description, string imageUrl, Action tapped = null)
        {
            _tapped = tapped;
            _name = name;
            _description = description;
            _imageUrl = imageUrl;
        }

        public override void Selected(UITableView tableView, Foundation.NSIndexPath path)
        {
            if (_tapped != null)
                _tapped();
            base.Selected(tableView, path);
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(ShowcaseTableViewCell.Key) as ShowcaseTableViewCell;
            if (cell == null)
            {
                cell = ShowcaseTableViewCell.Create();
            }

            cell.SetImage(_imageUrl);
            cell.Description = _description;
            cell.Name = _name;
            return cell;
        }

        public nfloat GetHeight(UIKit.UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            if (GetRootElement() == null)
                return 44f;

            var cell = GetRootElement().GetOffscreenCell(ShowcaseTableViewCell.Key, () => ShowcaseTableViewCell.Create());
            cell.Description = _description;
            cell.Name = _name;

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            cell.Bounds = new CGRect(0, 0, tableView.Bounds.Width, tableView.Bounds.Height);

            cell.SetNeedsLayout();
            cell.LayoutIfNeeded();

            return cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height + 1;
        }
    }
}

