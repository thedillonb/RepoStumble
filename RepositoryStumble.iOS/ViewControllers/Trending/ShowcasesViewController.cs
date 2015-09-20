using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;
using RepositoryStumble.Elements;
using RepositoryStumble.TableViewCells;
using UIKit;

namespace RepositoryStumble.ViewControllers.Trending
{
    public class ShowcasesViewController : ViewModelCollectionViewController<ShowcasesViewModel>
    {
        public ShowcasesViewController()
            : base(true, false)
        {
            Title = "Showcases";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.RegisterNibForCellReuse(ShowcaseTableViewCell.Nib, ShowcaseTableViewCell.Key);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 60f;

            this.BindList(ViewModel.Showcases, x => new ShowcaseElement(x.Name, x.Description, 
                x.ImageUrl, () => ViewModel.GoToShowcaseCommand.ExecuteIfCan(x)));
        }
    }
}

