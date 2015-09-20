using System;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;
using RepositoryStumble.Elements;
using RepositoryStumble.TableViewCells;
using UIKit;

namespace RepositoryStumble.ViewControllers.Trending
{
    public class ShowcaseViewController : ViewModelCollectionViewController<ShowcaseViewModel>
    {
        public ShowcaseViewController()
            : base(true, false)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.WhenAnyValue(x => x.Title).Subscribe(x => Title = x);

            TableView.RegisterNibForCellReuse(RepositoryTableViewCell.Nib, RepositoryTableViewCell.Key);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 80f;

            this.BindList(ViewModel.Repositories, x =>
                new RepositoryElement(x.Owner.Login, x.Name, x.Description, x.Owner.AvatarUrl, () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x)));
        }
    }
}