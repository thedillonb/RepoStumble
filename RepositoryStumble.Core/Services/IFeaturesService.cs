using System.Threading.Tasks;

namespace RepositoryStumble.Core.Services
{
    public interface IFeaturesService
    {
        bool ProEditionEnabled { get; }

        Task<string> GetProEditionPrice();

        Task EnableProEdition();
    }
}

