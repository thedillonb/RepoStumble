using ReactiveUI;
using RepositoryStumble.Core.Services;
using System.Linq;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class DislikedRepositoriesViewModel : BaseRepositoriesViewModel
    {
        protected readonly IApplicationService ApplicationService;

        public DislikedRepositoriesViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
            var repos = ApplicationService.Account.StumbledRepositories.Where(x => x.Liked != null && !x.Liked.Value).OrderByDescending(x => x.CreatedAt);
            Repositories.Reset(repos);
        }
    }
}

