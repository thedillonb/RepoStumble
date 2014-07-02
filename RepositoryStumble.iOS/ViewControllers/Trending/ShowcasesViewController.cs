using System;
using MonoTouch.UIKit;
using ReactiveUI;
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
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.Showcases.Changed.Subscribe(_ =>
            {
                var root = new RootElement(Title) { UnevenRows = true };
                var section = new Section();
                section.AddAll(ViewModel.Showcases.Select(x =>
                {
                    var e = new StyledMultilineElement(x.Name, x.Description, UITableViewCellStyle.Subtitle);
                    e.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    e.Tapped += () => ViewModel.GoToShowcaseCommand.ExecuteIfCan(x);
                    return e;
                }));
                root.Add(section);
                Root = root;
            });
        }
    }
}

