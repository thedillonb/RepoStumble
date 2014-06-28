using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using RepositoryStumble.Core.Data;
using RepositoryStumble.Core.ViewModels.Languages;
using ReactiveUI;
using System.Linq;

namespace RepositoryStumble.ViewControllers.Languages
{
    public class LanguagesViewController : ViewModelDialogViewController<LanguagesViewModel>
    {
        public LanguagesViewController(Language selectedLanguage)
        {
            ViewModel.SelectedLanguage = selectedLanguage;
            ViewModel.WhenAnyValue(x => x.Languages).Subscribe(x =>
            {
                var root = new RootElement("Languages");
                var sec = new Section();
                root.Add(sec);

                foreach (var l in x.OrderBy(y => y.Name))
                {
                    var closureL = l;
                    var el = new StyledStringElement(l.Name);
                    el.Tapped += () => {
                        ViewModel.SelectedLanguage = closureL;
                        NavigationController.PopViewControllerAnimated(true);
                    };

                    if (ViewModel.SelectedLanguage != null && closureL.Slug.Equals(ViewModel.SelectedLanguage.Slug))
                        el.Accessory = UITableViewCellAccessory.Checkmark;
                    sec.Add(el);
                }

                Root = root;
            });
        }
    }
}

