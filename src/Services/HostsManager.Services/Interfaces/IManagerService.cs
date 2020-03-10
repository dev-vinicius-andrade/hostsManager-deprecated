using System.Collections.Generic;
using HostsManager.Services.Entities;

namespace HostsManager.Services.Interfaces
{
    public interface IManagerService
    {
        KeyValuePair<string, Profile> GetActiveProfile();
        Dictionary<string, Profile> GetProfiles();
        KeyValuePair<string, Profile> GetDefaultConfiguration();
        void ActivateProfile(string profileName);
    }
}