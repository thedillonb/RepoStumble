using System;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Services;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;
using Xamarin.Utilities.Core.ReactiveAddons;
using RepositoryStumble.Core.ViewModels.Repositories;
using System.Reactive.Linq;
using System.Collections.Generic;
using RepositoryStumble.Core.ViewModels.Languages;
using RepositoryStumble.Core.Data;
using System.Linq;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class TrendingViewModel : BaseViewModel, ILoadableViewModel
    {
        private const string TrendingUrl = "http://trending.codehub-app.com/trending";
        private readonly IJsonHttpClientService _jsonHttpClient;
        private readonly TimeModel[] _times = 
        {
            new TimeModel { Name = "Daily", Slug = "daily" },
            new TimeModel { Name = "Weekly", Slug = "weekly" },
            new TimeModel { Name = "Monthly", Slug = "monthly" }
        };
        private readonly Language _defaultLanguage = new Language { Name = "All Languages", Slug = null };

        public ReactiveCollection<TrendingRepositoryViewModel> Repositories { get; private set; }

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { this.RaiseAndSetIfChanged(ref _selectedLanguage, value); }
        }

        public IReactiveCommand<object> GoToRepositoryCommand { get; private set; }

        public IReactiveCommand<object> GoToLanguages { get; private set; }

        public IReactiveCommand LoadCommand { get; private set; }

        public TrendingViewModel(INetworkActivityService networkActivity, IJsonHttpClientService jsonHttpClient)
        {
            _jsonHttpClient = jsonHttpClient;
            SelectedLanguage = _defaultLanguage;

            Repositories = new ReactiveCollection<TrendingRepositoryViewModel>();
            Repositories.GroupFunc = x => x.Time;

            GoToRepositoryCommand = ReactiveCommand.Create();
            GoToRepositoryCommand.OfType<RepositoryModel>().Subscribe(x =>
            {
                var vm = CreateViewModel<RepositoryViewModel>();
                vm.RepositoryIdentifier = new BaseRepositoryViewModel.RepositoryIdentifierModel(x.Owner, x.Name);
                ShowViewModel(vm);
            });

            GoToLanguages = ReactiveCommand.Create();
            GoToLanguages.Subscribe(_ =>
            {
                var vm = CreateViewModel<LanguagesViewModel>();
                vm.ExtraLanguages.Add(_defaultLanguage);
                vm.SelectedLanguage = SelectedLanguage;
                vm.WhenAnyValue(x => x.SelectedLanguage).Skip(1).Subscribe(x => 
                {
                    SelectedLanguage = x;
                    vm.DismissCommand.ExecuteIfCan();
                });
                ShowViewModel(vm);
            });

            this.WhenAnyValue(x => x.SelectedLanguage).Skip(1).Subscribe(_ => LoadCommand.ExecuteIfCan());

            LoadCommand = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var tempRepos = new List<TrendingRepositoryViewModel>();
                foreach (var t in _times)
                {
                    var query = "?since=" + t.Slug;
                    if (SelectedLanguage != null && SelectedLanguage.Slug != null)
                        query += string.Format("&language={0}", SelectedLanguage.Slug);
                    var repos = await _jsonHttpClient.Get<List<RepositoryModel>>(TrendingUrl + query);
                    tempRepos.AddRange(repos.Select(x => new TrendingRepositoryViewModel { Repository = x, Time = t.Name } ));
                }
                Repositories.Reset(tempRepos);
            });

            LoadCommand.TriggerNetworkActivity(networkActivity);
        }

        public class TimeModel
        {
            public string Name { get; set; }
            public string Slug { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != typeof(TimeModel))
                    return false;
                var other = (TimeModel)obj;
                return Name == other.Name && Slug == other.Slug;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Name != null ? Name.GetHashCode() : 0) ^ (Slug != null ? Slug.GetHashCode() : 0);
                }
            }

        }

        public class RepositoryModel
        {
            public string Url { get; set; }
            public string Owner { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string AvatarUrl { get; set; }
            public int Stars { get; set; }
            public int Forks { get; set; }
        }

        public class TrendingRepositoryViewModel
        {
            public RepositoryModel Repository { get; set; }
            public string Time { get; set; }
        }
    }
}

