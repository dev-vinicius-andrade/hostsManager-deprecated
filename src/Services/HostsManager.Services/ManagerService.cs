using System;
using System.Collections.Generic;
using System.Linq;
using HostsManager.Services.Configuration;
using HostsManager.Services.Entities;
using HostsManager.Services.Handlers;
using HostsManager.Services.Helpers;
using HostsManager.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace HostsManager.Services
{

    internal class ManagerService : IManagerService
    {
        private readonly ConfigurationsHandler _configurationsHandler;
        public event EventHandler ConfigurationsChanged;
        private HostsConfigurations _hostsConfigurations;

        public ManagerService(IOptionsMonitor<HostsConfigurations> configurations,
            ConfigurationsHandler configurationsHandler)
        {
            _configurationsHandler = configurationsHandler;
            _hostsConfigurations =
                configurations.CurrentValue ?? throw new ArgumentNullException(nameof(configurations));
            AddDefaultConfiguration();
            PersistConfigurationsChanges(configurations);
        }

        private void PersistConfigurationsChanges(IOptionsMonitor<HostsConfigurations> configurationsOptions)
        {

            configurationsOptions.OnChange(updatedConfiguration =>
            {
                _hostsConfigurations = updatedConfiguration;
                AddDefaultConfiguration();
                NotifyConfigurationsChanged();
            });
        }


        private void AddDefaultConfiguration()
        {
            if (_hostsConfigurations == null)
                return;
            if(string.IsNullOrEmpty(_hostsConfigurations.HostsFileFolder))
                return;

            var (key, value) = GetDefaultConfiguration();

            if (!_hostsConfigurations.Profiles.ContainsKey(key))
                _hostsConfigurations.Profiles.Add(key, value);
        }

        public KeyValuePair<string, Profile> GetActiveProfile()
        {

            var activesProfile = _hostsConfigurations.Profiles.Where(p => p.Value.Active).ToList();
            if (!activesProfile.Any())
            {
                var hostService = BuildHostService();
                var activeProfileName = hostService.GetActiveProfileName() ?? Constants.DefaultProfileName;
                _hostsConfigurations.Profiles[activeProfileName].Active = true;
                return GetActiveProfile();
            }

            if (activesProfile.Count() > 1)
                throw new Exception("More than 1 profile is active, please check your configuration file.");

            return activesProfile.FirstOrDefault();
        }

        public Dictionary<string, Profile> GetProfiles() => _hostsConfigurations.Profiles;

        public KeyValuePair<string, Profile> GetDefaultConfiguration()
        {
            var hostService = BuildHostService();
            var defaultHosts = hostService.DefaultHosts();
            return new KeyValuePair<string, Profile>(
                key: Constants.DefaultProfileName,
                value: new Profile { Hosts = defaultHosts.ToList() });
        }

        public void ActivateProfile(string profileName)
        {
            var hostService = BuildHostService();
            var activeProfile = GetActiveProfile();
            if (!_hostsConfigurations.Profiles.ContainsKey(profileName))
                throw new Exception($"Profile name: {profileName} does not exists");


            _hostsConfigurations.Profiles[activeProfile.Key].Active = false;
            _hostsConfigurations.Profiles[profileName].Active = true;
            hostService.SetProfile(profileName, _hostsConfigurations.Profiles[profileName]);
        }

        public bool SaveProfile(string profileName, Profile profile)
        {
            if (!_hostsConfigurations.Profiles.ContainsKey(profileName))
                throw new Exception($"Profile name: {profileName} does not exists");
            _hostsConfigurations.Profiles[profileName] = profile;
            _configurationsHandler.SaveConfigurations(_hostsConfigurations);
            NotifyConfigurationsChanged();
            return true;
        }

        public bool AddProfile(string profileName, Profile profile)
        {
            if (_hostsConfigurations.Profiles.ContainsKey(profileName))
                throw new Exception($"Profile name: {profileName} already exists");
            _hostsConfigurations.Profiles.Add(profileName, profile);
            _configurationsHandler.SaveConfigurations(_hostsConfigurations);
            NotifyConfigurationsChanged();
            return true;
        }

        public bool DeleteProfile(string profileName)
        {
            if (!_hostsConfigurations.Profiles.ContainsKey(profileName))
                throw new Exception($"Profile name: {profileName} does not exists");
            var result = _hostsConfigurations.Profiles.Remove(profileName);
            _configurationsHandler.SaveConfigurations(_hostsConfigurations);
            NotifyConfigurationsChanged();
            return result;
        }

        public string GetConfigurationsFolder() => _configurationsHandler.GetConfigurationsFileFolder();
        private HostHandler BuildHostService() => new HostHandler(_hostsConfigurations.HostsFileFolder);
        public void NotifyConfigurationsChanged() => ConfigurationsChanged?.Invoke(this, null);

        public void ResetConfigurations()
        {
            _configurationsHandler.ResetConfigurations();
            NotifyConfigurationsChanged();
        }
    }
}
