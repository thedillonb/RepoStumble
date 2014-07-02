using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;
using System.Collections.Generic;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Languages
{
    public class LanguagesViewModel : LoadableViewModel
    {
        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { this.RaiseAndSetIfChanged(ref _selectedLanguage, value); }
        }

        public ReactiveList<Language> Languages { get; private set; }

        public LanguagesViewModel(IJsonHttpClientService jsonHttpClientService)
        {
            Languages = new ReactiveList<Language>();
            LoadCommand.RegisterAsyncTask(async t => 
                Languages.Reset((await jsonHttpClientService.Get<List<Language>>("http://codehub-trending.herokuapp.com/languages"))));
        }
    }
}

