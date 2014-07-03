using System;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;
using System.Collections.Generic;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Languages
{
    public class LanguagesViewModel : LoadableViewModel
    {
        private readonly ReactiveList<Language> _languages = new ReactiveList<Language>();

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { this.RaiseAndSetIfChanged(ref _selectedLanguage, value); }
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set { this.RaiseAndSetIfChanged(ref _searchKeyword, value); }
        }

        public IReadOnlyReactiveList<Language> Languages { get; private set; }

        public LanguagesViewModel(IJsonHttpClientService jsonHttpClientService)
        {
            Languages = _languages.CreateDerivedCollection(
                x => x, 
                x => x.Name.StartsWith(SearchKeyword ?? string.Empty, StringComparison.Ordinal), 
                signalReset: this.WhenAnyValue(x => x.SearchKeyword));

            LoadCommand.RegisterAsyncTask(async t => 
                _languages.Reset((await jsonHttpClientService.Get<List<Language>>("http://trending.codehub-app.com/languages"))));
        }
    }
}

