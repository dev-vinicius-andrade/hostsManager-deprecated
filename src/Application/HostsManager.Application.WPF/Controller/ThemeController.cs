using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using HostsManager.Application.WPF.Configuration;
using HostsManager.Application.WPF.Helpers;

namespace HostsManager.Application.WPF.Controller
{
    public class ThemeController
    {

        private readonly Dictionary<string, Theme> _themes;
        private KeyValuePair<string, Theme> _activeTheme;
        public ThemeController(Dictionary<string, Theme> themes)
        {
            _themes = themes.ConvertToCaseInSensitive();
            _activeTheme = GetActiveTheme();
            _activeTheme.Value.ColorsConfigurations = _activeTheme.Value.ColorsConfigurations.ConvertToCaseInSensitive();
        }

        private KeyValuePair<string, Theme> GetActiveTheme() => _themes.FirstOrDefault(t => t.Value.Active);


        public void ActivateTheme(string name)
        {
            if (!_themes.ContainsKey(name))
                throw new Exception($"There isn't a theme named {name}");
            DeactivateActiveTheme();
            _themes[name].Active = true;
            _activeTheme = new KeyValuePair<string, Theme>(name,_themes[name]);
        }

        private void DeactivateActiveTheme()
        {
            _themes[_activeTheme.Key].Active = false;
        }
        public Color GetColor(string name)
        {
            if (_activeTheme.Value == null)
                throw new Exception("There's no active theme");



            if (!_activeTheme.Value.ColorsConfigurations.ContainsKey(name))
                throw new NullReferenceException("There's no color with this name in the active theme");
            return _activeTheme.Value.ColorsConfigurations[name].ToHexadecimalColor();
        }

    }
}
