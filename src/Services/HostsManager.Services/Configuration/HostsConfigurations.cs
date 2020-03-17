using System.Collections.Generic;
using HostsManager.Services.Entities;

namespace HostsManager.Services.Configuration
{
    public class HostsConfigurations
    {
        public HostsConfigurations()
        {
            HostsFileFolder = string.Empty;
            DefaultHosts = new List<Hosts>();
            Profiles = new Dictionary<string, Profile>();
        }

        public string HostsFileFolder { get; set; }
        public List<Hosts> DefaultHosts { get; set; }

        public Dictionary<string, Profile> Profiles { get; set; }
    }
}
