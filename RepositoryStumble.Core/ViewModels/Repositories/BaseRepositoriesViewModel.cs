using RepositoryStumble.Core.Data;
using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public abstract class BaseRepositoriesViewModel : ReactiveObject
    {
        public ReactiveList<StumbledRepository> Repositories { get; private set; }

        protected BaseRepositoriesViewModel()
        {
            Repositories = new ReactiveList<StumbledRepository>();
        }
    }
}

