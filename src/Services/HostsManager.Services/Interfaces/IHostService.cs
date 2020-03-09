using System.Collections.Generic;
using HostsManager.Application.Entities;

namespace HostsManager.Application.Interfaces
{
    public interface IHostService
    {
        IReadOnlyList<Hosts> DefaultHosts();
        void RollbackHostsFile();
        void SetProfile(string profileName, Profile profile);
    }
}