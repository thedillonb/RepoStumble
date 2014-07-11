using ReactiveUI;
using RepositoryStumble.Core.Services;
using System.Linq;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class LikedRepositoriesViewModel : BaseRepositoriesViewModel
    {
        public LikedRepositoriesViewModel(IApplicationService applicationService)
        {
            var repos = applicationService.Account.StumbledRepositories.Where(x => x.Liked != null && x.Liked.Value).OrderByDescending(x => x.CreatedAt);
            RepositoryCollection.Reset(repos);
        }
    }
}

