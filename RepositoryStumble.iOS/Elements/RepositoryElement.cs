using System;
using Xamarin.Utilities.DialogElements;
using RepositoryStumble.TableViewCells;
using UIKit;

namespace RepositoryStumble.Elements
{
    public class RepositoryElement : Element
    {
        private readonly string _name;
        private readonly string _owner;
        private readonly string _description;
        private readonly string _imageUrl;
        private readonly Action _tapped;

        public RepositoryElement(string owner, string name, string description, string imageUrl, Action tapped = null)
        {
            _tapped = tapped;
            _owner = owner;
            _name = name;
            _description = description;
            _imageUrl = imageUrl;
        }

        public override void Selected(UITableView tableView, Foundation.NSIndexPath path)
        {
            _tapped?.Invoke();
            base.Selected(tableView, path);
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(RepositoryTableViewCell.Key) as RepositoryTableViewCell;
            cell.Set(_name, _owner, _description, _imageUrl);
            return cell;
        }
    }
}

