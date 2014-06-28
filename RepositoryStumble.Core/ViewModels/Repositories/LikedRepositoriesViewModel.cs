using ReactiveUI;
using RepositoryStumble.Core.Services;
using System.Linq;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class LikedRepositoriesViewModel : BaseRepositoriesViewModel
    {
        protected readonly IApplicationService ApplicationService;

        public LikedRepositoriesViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
            var repos = ApplicationService.Account.StumbledRepositories.Where(x => x.Liked != null && x.Liked.Value).OrderByDescending(x => x.CreatedAt);
            Repositories.Reset(repos);
        }
    }
}

