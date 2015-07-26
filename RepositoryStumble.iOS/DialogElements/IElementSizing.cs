using UIKit;
using Foundation;
using System;

namespace Xamarin.Utilities.DialogElements
{
    public interface IElementSizing 
    {
        nfloat GetHeight (UITableView tableView, NSIndexPath indexPath);
    }
}

