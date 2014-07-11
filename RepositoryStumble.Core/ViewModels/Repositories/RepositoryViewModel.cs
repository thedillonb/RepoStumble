using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class RepositoryViewModel : BaseRepositoryViewModel
    {
        public RepositoryViewModel(IApplicationService applicationService)
            : base(applicationService)
        {
        }
    }
}

