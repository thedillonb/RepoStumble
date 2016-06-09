using System;
using UIKit;
using RepositoryStumble.Core.ViewModels.Languages;
using Xamarin.Utilities.DialogElements;
using ReactiveUI;

namespace RepositoryStumble.ViewControllers.Languages
{
    public class LanguagesViewController : ViewModelCollectionViewController<LanguagesViewModel>
    {
        public LanguagesViewController()
        {
            Title = "Languages";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SearchTextChanging.Subscribe(x => ViewModel.SearchKeyword = x);

            this.BindList(ViewModel.Languages, x =>
            {
                var el = new StringElement(x.Name);
                el.Tapped += () => ViewModel.SelectedLanguage = x;
                if (ViewModel.SelectedLanguage != null && string.Equals(x.Slug, ViewModel.SelectedLanguage.Slug))
                    el.Accessory = UITableViewCellAccessory.Checkmark;
                return el;
            });
        }
    }
}

