using System.Collections.Generic;

namespace HostsManager.Application.WPF.Configuration
{
    public class Theme
    {
        public bool Active { get; set; }
        public Dictionary<string,string> ColorsConfigurations { get; set; }
    }
}