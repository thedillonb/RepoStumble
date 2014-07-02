using System;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using System.Collections.Generic;
using Xamarin.Utilities.Core.ViewModels;
using System.Reactive.Linq;
using RepositoryStumble.Core.Services;
using RepositoryStumble.Core.ViewModels.Languages;

namespace RepositoryStumble.Core.ViewModels.Interests
{
    public class AddInterestViewModel : BaseViewModel
    {
        public static readonly string PopularInterestsPath = "popular_interests.json";

        public IReactiveCommand GoToLanguagesCommand { get; private set; }

        public IReactiveCommand DoneCommand { get; private set; }

        public ReactiveList<PopularInterest> PopularInterests { get; private set; }

        private string _keyword;
        public string Keyword
        {
            get { return _keyword; }
            set { this.RaiseAndSetIfChanged(ref _keyword, value); }
        }

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { this.RaiseAndSetIfChanged(ref _selectedLanguage, value); }
        }

        public AddInterestViewModel(IApplicationService applicationService, IJsonSerializationService jsonSerializationService)
        {
            DoneCommand = new ReactiveCommand();
            DoneCommand.Subscribe(_ =>
            {
                if (SelectedLanguage == null)
                    throw new Exception("You must select a language for your interest!");
                if (string.IsNullOrEmpty(Keyword))
                    throw new Exception("Please specify a keyword to go with your interest!");

                applicationService.Account.Interests.Insert(new Interest
                {
                    Language = _selectedLanguage.Name,
                    LanguageId = _selectedLanguage.Slug,
                    Keyword = _keyword
                });

                DismissCommand.ExecuteIfCan();
            });

            GoToLanguagesCommand = new ReactiveCommand();
            GoToLanguagesCommand.Subscribe(_ =>
            {
                var vm = CreateViewModel<LanguagesViewModel>();
                vm.SelectedLanguage = SelectedLanguage;
                vm.WhenAnyValue(x => x.SelectedLanguage).Skip(1).Subscribe(x => 
                {
                    SelectedLanguage = x;
                    vm.DismissCommand.ExecuteIfCan();
                });
                ShowViewModel(vm);
            });


            var str = System.IO.File.ReadAllText(PopularInterestsPath, System.Text.Encoding.UTF8);
            PopularInterests = new ReactiveList<PopularInterest>(jsonSerializationService.Deserialize<List<PopularInterest>>(str));

        }
    }
}

