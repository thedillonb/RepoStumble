using System;
using System.Linq;
using RepositoryStumble.Core.ViewModels.Interests;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;

namespace RepositoryStumble.ViewControllers.Interests
{
    public class AddInterestViewController : ViewModelDialogViewController<AddInterestViewModel>
    {
        public AddInterestViewController()
        {
            Title = "Add Interest";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var input = new InputElement("Keyword", string.Empty, string.Empty);
            input.Changed += (sender, e) => ViewModel.Keyword = input.Value;
            input.AutocorrectionType = UIKit.UITextAutocorrectionType.No;
            input.AutocapitalizationType = UIKit.UITextAutocapitalizationType.None;
            input.TextAlignment = UIKit.UITextAlignment.Right;

            NavigationItem.RightBarButtonItem = new UIKit.UIBarButtonItem(UIKit.UIBarButtonSystemItem.Done, (s, e) => 
            {
                input.ResignFirstResponder(false);
                ViewModel.DoneCommand.ExecuteIfCan();
            });

            var showLanguage = new StringElement("Language", string.Empty, UIKit.UITableViewCellStyle.Value1);
            showLanguage.Tapped += () =>
            {
                input.ResignFirstResponder(false);
                ViewModel.GoToLanguagesCommand.ExecuteIfCan();
            };
            showLanguage.Accessory = UIKit.UITableViewCellAccessory.DisclosureIndicator;

            var sec2 = new Section("Add New Interest") { showLanguage, input };
            var sec3 = new Section("Popular Interests");
            Root.Reset(sec2, sec3);

            ViewModel.WhenAnyValue(x => x.PopularInterests).Subscribe(_ => sec3.Reset(ViewModel.PopularInterests.OrderBy(x => x.Keyword).Select(p =>
            {
                var item = new StringElement(p.Keyword, p.Language.Name, UIKit.UITableViewCellStyle.Subtitle);
                item.Accessory = UIKit.UITableViewCellAccessory.DisclosureIndicator;
                item.Tapped += () =>
                {
                    ViewModel.Keyword = p.Keyword;
                    ViewModel.SelectedLanguage = p.Language;
                    ViewModel.DoneCommand.ExecuteIfCan();
                };
                return item;
            })));

            ViewModel.WhenAnyValue(x => x.SelectedLanguage).Subscribe(x =>
            {
                showLanguage.Value = x == null ? string.Empty : x.Name;
                Root.Reload(showLanguage, UIKit.UITableViewRowAnimation.None);
            });
        }
    }
}

