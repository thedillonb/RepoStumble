using ReactiveUI;
using RepositoryStumble.Core.Services;
using System.Linq;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class DislikedRepositoriesViewModel : BaseRepositoriesViewModel
    {

        public DislikedRepositoriesViewModel(IApplicationService applicationService)
        {
            var repos = applicationService.Account.StumbledRepositories.Where(x => x.Liked != null && !x.Liked.Value).OrderByDescending(x => x.CreatedAt);
            RepositoryCollection.Reset(repos);
        }
    }
}

