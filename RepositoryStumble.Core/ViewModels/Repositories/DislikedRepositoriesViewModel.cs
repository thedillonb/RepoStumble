using ReactiveUI;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class DislikedRepositoriesViewModel : BaseRepositoriesViewModel
    {
        public DislikedRepositoriesViewModel(IApplicationService applicationService)
        {
            this.WhenActivated(d =>
            {
                var repos = applicationService.Account.StumbledRepositories.Query
                    .Where(x => x.Liked == false)
                    .OrderByDescending(x => x.CreatedAt);
                RepositoryCollection.Reset(repos);
            });
        }
    }
}

