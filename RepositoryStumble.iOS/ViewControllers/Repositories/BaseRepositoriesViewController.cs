using System;
using RepositoryStumble.Core.ViewModels.Repositories;
using RepositoryStumble.Elements;
using System.Linq;
using Xamarin.Utilities.DialogElements;
using System.Reactive.Linq;
using System.Collections.Specialized;
using ReactiveUI;
using RepositoryStumble.TableViewCells;
using UIKit;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public abstract class BaseRepositoriesViewController<TViewModel> : ViewModelCollectionViewController<TViewModel> where TViewModel : BaseRepositoriesViewModel
    {
        protected BaseRepositoriesViewController()
            : base(unevenRows: true)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SearchTextChanging.Where(x => ViewModel != null).Subscribe(x => ViewModel.SearchKeyword = x);

            TableView.RegisterNibForCellReuse(RepositoryTableViewCell.Nib, RepositoryTableViewCell.Key);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 80f;

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.Repositories.Changed.StartWith(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)))
                .Switch()
                .Subscribe(_ =>
                {
                    var sec = new Section();
                    sec.AddAll(
                        from x in ViewModel.Repositories
                        select new RepositoryElement(x.Owner, x.Name, x.Description, x.ImageUrl, () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x)));
                    Root.Reset(sec);
                });
        }
    }
}

