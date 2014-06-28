using System;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using System.Collections.Generic;
using Xamarin.Utilities.Core.ViewModels;
using System.Reactive.Linq;
using RepositoryStumble.Core.Services;

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
            var doneObservable = this.WhenAnyValue(x => x.Keyword, y => y.SelectedLanguage, (x, y) => new { x, y }).Select(x => !string.IsNullOrEmpty(x.x) && x.y != null);

            GoToLanguagesCommand = new ReactiveCommand();
            DoneCommand = new ReactiveCommand(doneObservable);

            DoneCommand.Subscribe(_ =>
            {
                if (_selectedLanguage == null || string.IsNullOrWhiteSpace(_keyword))
                    return;

                applicationService.Account.Interests.Insert(new Interest
                {
                    Language = _selectedLanguage.Name,
                    LanguageId = _selectedLanguage.Slug,
                    Keyword = _keyword
                });

//                var gameScore = new ParseObject("Interest");
//                gameScore["language"] = _selectedLanguage.Name;
//                gameScore["keyword"] = _keyword;
//                gameScore.SaveAsync();
            });

            var str = System.IO.File.ReadAllText(PopularInterestsPath, System.Text.Encoding.UTF8);
            PopularInterests = new ReactiveList<PopularInterest>(jsonSerializationService.Deserialize<List<PopularInterest>>(str));

        }
    }
}

