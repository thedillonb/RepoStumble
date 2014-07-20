using ReactiveUI;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class LikedRepositoriesViewModel : BaseRepositoriesViewModel
    {
        public LikedRepositoriesViewModel(IApplicationService applicationService)
        {
            this.WhenActivated(d =>
            {
                var repos = applicationService.Account.StumbledRepositories.Query
                    .Where(x => x.Liked == true)
                    .OrderByDescending(x => x.CreatedAt);
                RepositoryCollection.Reset(repos);
            });
        }
    }
}

