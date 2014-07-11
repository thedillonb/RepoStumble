using ReactiveUI;
using RepositoryStumble.Core.Services;
using System.Linq;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class HistoryViewModel : BaseRepositoriesViewModel
    {
        protected readonly IApplicationService ApplicationService;

        public HistoryViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
            RepositoryCollection.Reset(ApplicationService.Account.StumbledRepositories.OrderByDescending(x => x.CreatedAt));
        }
    }
}

