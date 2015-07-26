using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public class RepositoryViewModel : BaseRepositoryViewModel
    {
        public RepositoryViewModel(IApplicationService applicationService, INetworkActivityService networkActivity)
            : base(applicationService, networkActivity)
        {
        }
    }
}

