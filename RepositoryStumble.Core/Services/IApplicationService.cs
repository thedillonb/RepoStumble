using System;
using RepositoryStumble.Core.Data;

namespace RepositoryStumble.Core.Services
{
    public interface IApplicationService
    {
        Account Account { get; }

        void Logout();

        Octokit.IGitHubClient Client { get; }

        bool Load();

        IObservable<StumbledRepository> RepositoryAdded { get; }
    }
}

