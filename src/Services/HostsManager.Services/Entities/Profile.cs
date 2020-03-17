
using System.Collections.Generic;

namespace HostsManager.Services.Entities
{
    public class Profile
    {
        public Profile()
        {
            Active = false;
            Hosts = new List<Hosts>();
        }
        public List<Hosts> Hosts { get; set; }
        public bool Active { get; set; }

    }
}
