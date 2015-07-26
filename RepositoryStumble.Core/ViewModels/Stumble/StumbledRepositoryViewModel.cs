using RepositoryStumble.Core.ViewModels.Repositories;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Stumble
{
    public class StumbledRepositoryViewModel : BaseRepositoryViewModel
    {
        public StumbledRepositoryViewModel(IApplicationService applicationService, INetworkActivityService networkActivity)
            : base(applicationService, networkActivity)
        {
        }
    }
}