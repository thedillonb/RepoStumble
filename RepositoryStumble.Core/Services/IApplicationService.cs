using System;
using RepositoryStumble.Core.Data;

namespace RepositoryStumble.Core.Services
{
    public interface IApplicationService
    {
        Account Account { get; }

        void Logout();

        GitHubSharp.Client Client { get; }

        bool Load();

        IObservable<StumbledRepository> RepositoryAdded { get; }
    }
}

