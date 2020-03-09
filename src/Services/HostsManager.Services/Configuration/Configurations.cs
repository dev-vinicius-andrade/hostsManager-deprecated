using System.Collections.Generic;
using HostsManager.Application.Entities;

namespace HostsManager.Application.Configuration
{
    public class Configurations
    {
        private Dictionary<string, Profile> profiles;

        public Configurations()
        {
            HostsFileFolder = string.Empty;
            DefaultHosts = new List<Hosts>();
            Profiles = new Dictionary<string, Profile>();
        }

        public string HostsFileFolder { get; set; }
        public List<Hosts> DefaultHosts { get; set; }

        public Dictionary<string, Profile> Profiles
        {
            get;set;
        }
    }
}
