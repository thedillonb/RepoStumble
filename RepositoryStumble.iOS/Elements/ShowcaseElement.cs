using System;
using Xamarin.Utilities.DialogElements;
using RepositoryStumble.TableViewCells;
using UIKit;

namespace RepositoryStumble.Elements
{
    public class ShowcaseElement : Element
    {
        private readonly string _name;
        private readonly string _description;
        private readonly string _imageUrl;
        private readonly Action _tapped;

        public ShowcaseElement(string name, string description, string imageUrl, Action tapped = null)
        {
            _tapped = tapped;
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
            var cell = tv.DequeueReusableCell(ShowcaseTableViewCell.Key) as ShowcaseTableViewCell;
            cell.Set(_name, _description, _imageUrl);
            return cell;
        }
    }
}

