
using System.Collections.Generic;

namespace HostsManager.Application.Entities
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

        //public string Name { get; private set; }
        //public void SetName(string name)
        //{
        //    if (!string.IsNullOrEmpty(name))
        //        Name = name;
        //}

    }
}
