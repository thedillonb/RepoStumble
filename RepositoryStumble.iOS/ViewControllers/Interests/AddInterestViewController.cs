using System;
using MonoTouch.Dialog;
using System.Linq;
using RepositoryStumble.Core.ViewModels.Interests;
using RepositoryStumble.ViewControllers.Languages;
using ReactiveUI;

namespace RepositoryStumble.ViewControllers.Interests
{
    public class AddInterestViewController : ViewModelDialogViewController<AddInterestViewModel>
    {
		private readonly InputElement _input = new InputElement("Keyword", string.Empty, string.Empty);

        public AddInterestViewController()
			: base(MonoTouch.UIKit.UITableViewStyle.Grouped)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.RightBarButtonItem = new MonoTouch.UIKit.UIBarButtonItem(MonoTouch.UIKit.UIBarButtonSystemItem.Done, (s, e) => 
            {
                _input.ResignFirstResponder(false);
                ViewModel.DoneCommand.ExecuteIfCan();
            });

            _input.Changed += (sender, e) => ViewModel.Keyword = _input.Value;
            _input.AutocorrectionType = MonoTouch.UIKit.UITextAutocorrectionType.No;
            _input.AutocapitalizationType = MonoTouch.UIKit.UITextAutocapitalizationType.None;
            _input.TextAlignment = MonoTouch.UIKit.UITextAlignment.Right;

            var showLanguage = new StyledStringElement("Language", string.Empty, MonoTouch.UIKit.UITableViewCellStyle.Value1);
            showLanguage.Tapped += () =>
            {
                _input.ResignFirstResponder(false);
                ViewModel.GoToLanguagesCommand.ExecuteIfCan();
            };
            showLanguage.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator;

            var sec2 = new Section("Add New Interest") { showLanguage, _input };
            var sec3 = new Section("Popular Interests");
            Root = new RootElement("Add Interest") { sec2, sec3 };

            ViewModel.WhenAnyValue(x => x.PopularInterests).Subscribe(_ => sec3.Reset(ViewModel.PopularInterests.OrderBy(x => x.Keyword).Select(p =>
            {
                var item = new StyledStringElement(p.Keyword, p.Language.Name, MonoTouch.UIKit.UITableViewCellStyle.Subtitle);
                item.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator;
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
                Root.Reload(showLanguage, MonoTouch.UIKit.UITableViewRowAnimation.None);
            });
        }
    }
}

