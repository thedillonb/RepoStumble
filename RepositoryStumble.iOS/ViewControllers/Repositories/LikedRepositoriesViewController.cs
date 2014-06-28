using RepositoryStumble.Core.ViewModels.Repositories;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public class LikedRepositoriesViewController : BaseRepositoriesViewController<LikedRepositoriesViewModel>
    {
        public LikedRepositoriesViewController()
        {
            Title = "Liked";
        }
    }
}

