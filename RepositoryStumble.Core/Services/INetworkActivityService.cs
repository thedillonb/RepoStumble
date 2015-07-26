namespace RepositoryStumble.Core.Services
{
    public interface INetworkActivityService
    {
        void PushNetworkActive();

        void PopNetworkActive();
    }
}

