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
            _input.Changed += (object sender, EventArgs e) => ViewModel.Keyword = _input.Value;
			_input.AutocorrectionType = MonoTouch.UIKit.UITextAutocorrectionType.No;
			_input.AutocapitalizationType = MonoTouch.UIKit.UITextAutocapitalizationType.None;
			_input.TextAlignment = MonoTouch.UIKit.UITextAlignment.Right;

            ViewModel.GoToLanguagesCommand.Subscribe(_ =>
            {
                _input.ResignFirstResponder(false);
                var l = new LanguagesViewController(ViewModel.SelectedLanguage);
                l.ViewModel.WhenAnyValue(x => x.SelectedLanguage).Subscribe(x => ViewModel.SelectedLanguage = x);
                NavigationController.PushViewController(l, true);
            });

            NavigationItem.RightBarButtonItem = new MonoTouch.UIKit.UIBarButtonItem(MonoTouch.UIKit.UIBarButtonSystemItem.Done, (s, e) => Create());;

            var showLanguage = new StyledStringElement("Language", string.Empty, MonoTouch.UIKit.UITableViewCellStyle.Value1);
            showLanguage.Tapped += () => ViewModel.GoToLanguagesCommand.ExecuteIfCan();
            showLanguage.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator;

            var sec2 = new Section("Add New Interest");
            sec2.Add(showLanguage);
            sec2.Add(_input);

            var sec3 = new Section("Popular Interests");
            ViewModel.WhenAnyValue(x => x.PopularInterests).Subscribe(_ => sec3.Reset(ViewModel.PopularInterests.OrderBy(x => x.Keyword).Select(p =>
            {
                var item = new StyledStringElement(p.Keyword, p.Language.Name, MonoTouch.UIKit.UITableViewCellStyle.Subtitle);
                item.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator;
                item.Tapped += () =>
                {
                    ViewModel.Keyword = p.Keyword;
                    ViewModel.SelectedLanguage = p.Language;
                    Create();
                };
                return item;
            })));

            Root = new RootElement("Add Interest") { sec2, sec3 };

            ViewModel.DismissCommand.Subscribe(_ => NavigationController.PopViewControllerAnimated(true));
        }

		private void Create()
		{
            _input.ResignFirstResponder(false);
            ViewModel.DoneCommand.ExecuteIfCan();
		}
    }
}

