using System;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Services;
using System.Collections.Generic;
using RepositoryStumble.Core.Data;
using System.Threading.Tasks;

namespace RepositoryStumble.Core.ViewModels.Languages
{
    public class LanguagesViewModel : LoadableViewModel
    {
        protected readonly IApplicationService ApplicationService;

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { this.RaiseAndSetIfChanged(ref _selectedLanguage, value); }
        }

        private IEnumerable<Language> _languages;
        public IEnumerable<Language> Languages
        {
            get { return _languages; }
            private set { this.RaiseAndSetIfChanged(ref _languages, value); }
        }

        public LanguagesViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }
    }
}

