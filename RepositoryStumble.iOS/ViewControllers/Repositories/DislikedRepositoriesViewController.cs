using RepositoryStumble.Core.ViewModels.Repositories;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public class DislikedRepositoriesViewController : BaseRepositoriesViewController<DislikedRepositoriesViewModel>
    {
        public DislikedRepositoriesViewController()
        {
            Title = "Dislikes";
        }
    }
}

