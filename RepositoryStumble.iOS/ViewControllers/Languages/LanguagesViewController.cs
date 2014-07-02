using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using RepositoryStumble.Core.Data;
using RepositoryStumble.Core.ViewModels.Languages;
using System.Linq;

namespace RepositoryStumble.ViewControllers.Languages
{
    public class LanguagesViewController : ViewModelDialogViewController<LanguagesViewModel>
    {
        public override void ViewDidLoad()
        {
            Title = "Langauges";

            base.ViewDidLoad();

            ViewModel.Languages.Changed.Subscribe(_ =>
            {
                var root = new RootElement(Title);
                var sec = new Section();
                root.Add(sec);

                foreach (var l in ViewModel.Languages.OrderBy(y => y.Name))
                {
                    var closureL = l;
                    var el = new StyledStringElement(l.Name);
                    el.Tapped += () => ViewModel.SelectedLanguage = closureL;
                    if (ViewModel.SelectedLanguage != null && closureL.Slug.Equals(ViewModel.SelectedLanguage.Slug))
                        el.Accessory = UITableViewCellAccessory.Checkmark;
                    sec.Add(el);
                }

                Root = root;
            });
        }
    }
}

