using RepositoryStumble.Core.Data;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public abstract class BaseRepositoriesViewModel : BaseViewModel
    {
        public ReactiveList<StumbledRepository> Repositories { get; private set; }

        protected BaseRepositoriesViewModel()
        {
            Repositories = new ReactiveList<StumbledRepository>();
        }
    }
}

