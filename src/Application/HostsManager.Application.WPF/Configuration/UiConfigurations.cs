using System.Collections.Generic;

namespace HostsManager.Application.WPF.Configuration
{
    public class UiConfigurations
    {
        public MainWindowConfigurations MainWindowConfigurations { get; set; }
        public ProfileWindowConfigurations ProfileWindowConfigurations { get; set; }
        public Dictionary<string, Theme>  ThemeConfiguration { get; set; }
    }
}