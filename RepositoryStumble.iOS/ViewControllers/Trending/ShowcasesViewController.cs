using System;
using RepositoryStumble.Core.ViewModels.Trending;
using MonoTouch.Dialog;
using System.Linq;

namespace RepositoryStumble.ViewControllers.Trending
{
    public class ShowcasesViewController : ViewModelDialogViewController<ShowcasesViewModel>
    {
        public ShowcasesViewController()
        {
            Title = "Showcases";

            ViewModel.Showcases.Changed.Subscribe(_ =>
            {
                var root = new RootElement(Title) { UnevenRows = true };
                var section = new Section();
                section.AddAll(ViewModel.Showcases.Select(x => new StyledStringElement(x.Name, x.Description, MonoTouch.UIKit.UITableViewCellStyle.Subtitle)));
                root.Add(section);
                Root = root;
            });
        }
    }
}

