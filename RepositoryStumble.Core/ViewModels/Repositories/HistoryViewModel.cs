using ReactiveUI;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class HistoryViewModel : BaseRepositoriesViewModel
    {
        protected readonly IApplicationService ApplicationService;

        public HistoryViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
            RepositoryCollection.Reset(ApplicationService.Account.StumbledRepositories.Query.OrderByDescending(x => x.CreatedAt));
        }
    }
}

