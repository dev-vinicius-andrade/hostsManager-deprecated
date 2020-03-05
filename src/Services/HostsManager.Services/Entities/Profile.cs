
using System.Collections.Generic;

namespace HostsManager.Application.Entities
{
    public class Profile
    {
        public Profile()
        {
            Status = false;
            Hosts = new List<Hosts>();
        }
        protected bool Status { get; set; }
        public List<Hosts> Hosts { get; set; }
    }
}
