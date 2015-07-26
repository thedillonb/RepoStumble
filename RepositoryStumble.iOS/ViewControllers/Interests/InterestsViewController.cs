using System;
using UIKit;
using System.Linq;
using RepositoryStumble.Core.ViewModels.Interests;
using RepositoryStumble.Elements;
using ReactiveUI;
using System.Reactive.Linq;
using System.Collections.Specialized;
using Xamarin.Utilities.DialogElements;

namespace RepositoryStumble.ViewControllers.Interests
{
    public class InterestsViewController : ViewModelDialogViewController<InterestsViewModel>
    {
        public InterestsViewController()
            : base(style: UITableViewStyle.Plain)
        {
            Title = "Interests";
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            ViewModel.Interests.Changed.StartWith((NotifyCollectionChangedEventArgs)null).Subscribe(_ =>
            {
                var sec = new Section();
                sec.AddAll(ViewModel.Interests.Select(x => new InterestElement(x, () => ViewModel.GoToStumbleInterestCommand.ExecuteIfCan(x))));
                Root.Reset(sec);
            });

			NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, 
                (s, e) => ViewModel.GoToAddInterestCommand.ExecuteIfCan());
		}

		private void Delete(Element e)
		{
			var ie = e as InterestElement;
            ViewModel.DeleteInterestCommand.ExecuteIfCan(ie.Interest);
		}
            
		public override Source CreateSizingSource(bool unevenRows)
		{
			return new EditSource(this);
		}

		private class EditSource : Source
		{
			private readonly InterestsViewController _parent;
			public EditSource(InterestsViewController dvc) 
                : base (dvc)
			{
				_parent = dvc;
			}

			public override bool CanEditRow(UITableView tableView, Foundation.NSIndexPath indexPath)
			{
				return (indexPath.Section == 0);
			}

			public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
			{
				if (indexPath.Section == 0)
					return UITableViewCellEditingStyle.Delete;
				return UITableViewCellEditingStyle.None;
			}

			public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
			{
				switch (editingStyle)
				{
					case UITableViewCellEditingStyle.Delete:
						var section = _parent.Root[indexPath.Section];
						var element = section[indexPath.Row];
						_parent.Delete(element);
						section.Remove(element);
						break;
				}
			}
		}
    }
}

