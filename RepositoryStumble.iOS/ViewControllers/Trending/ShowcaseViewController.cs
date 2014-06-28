using System;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Trending;

namespace RepositoryStumble.ViewControllers.Trending
{
    public class ShowcaseViewController : ViewModelDialogViewController<ShowcaseViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.WhenAnyValue(x => x.Title).Subscribe(x => Title = x);
            ViewModel.Repositories.Changed.Subscribe(_ =>
            {
                var root = new RootElement(Title) {UnevenRows = true};
                var sec = new Section();
                sec.AddAll(ViewModel.Repositories.Select(x =>
                {
                    var element = new StyledMultilineElement(x.Owner + "/" + x.Name, x.Description,
                        UITableViewCellStyle.Subtitle);
                    element.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    element.Tapped += () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x);
                    return element;
                }));
                root.Add(sec);
                Root = root;
            });
        }
    }
}