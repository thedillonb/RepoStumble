using System;
using RepositoryStumble.Core.Data;
using ReactiveUI;
using System.Reactive.Linq;
using RepositoryStumble.Core.ViewModels.Stumble;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public abstract class BaseRepositoriesViewModel : BaseViewModel
    {
        protected ReactiveList<StumbledRepository> RepositoryCollection { get; private set; }

        public IReadOnlyReactiveCollection<StumbledRepository> Repositories { get; private set; }

        public IReactiveCommand<object> GoToRepositoryCommand { get; private set; }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set { this.RaiseAndSetIfChanged(ref _searchKeyword, value); }
        }

        protected BaseRepositoriesViewModel()
        {
            GoToRepositoryCommand = ReactiveCommand.Create();
            GoToRepositoryCommand.OfType<StumbledRepository>().Subscribe(x =>
            {
                var vm = CreateViewModel<StumbledRepositoryViewModel>();
                vm.RepositoryIdentifier = new BaseRepositoryViewModel.RepositoryIdentifierModel(x.Owner, x.Name);
                ShowViewModel(vm);
            });

            RepositoryCollection = new ReactiveList<StumbledRepository>();
            Repositories = RepositoryCollection.CreateDerivedCollection(
                x => x, 
                x => x.Owner.StartsWith(SearchKeyword ?? string.Empty, System.StringComparison.OrdinalIgnoreCase) ||
                x.Name.StartsWith(SearchKeyword ?? string.Empty, System.StringComparison.OrdinalIgnoreCase), 
                signalReset: this.WhenAnyValue(x => x.SearchKeyword));
        }
    }
}

