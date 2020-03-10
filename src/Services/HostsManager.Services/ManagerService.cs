using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HostsManager.Services.Configuration;
using HostsManager.Services.Entities;
using HostsManager.Services.Handlers;
using HostsManager.Services.Helpers;
using HostsManager.Services.Interfaces;

namespace HostsManager.Services
{
    public class ManagerService : IManagerService
    {
        private readonly Configurations _configurations;
        public ManagerService()
        {
            _configurations = GetConfigurations();
            AddDefaultConfiguration();
        }

        private void AddDefaultConfiguration()
        {
            if (_configurations == null)
                return;

            var (key, value) = GetDefaultConfiguration();
            _configurations.Profiles.Add(key, value);

        }
        private Configurations GetConfigurations()
        {
            try
            {
                var service = new ConfigurationsHandler();
                var configurations = service.GetConfigurations();

                return configurations;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public KeyValuePair<string, Profile> GetActiveProfile()
        {

            var activesProfile = _configurations.Profiles.Where(p => p.Value.Active).ToList();
            if (!activesProfile.Any())
            {
                var hostService = BuildHostService();
                var activeProfileName = hostService.GetActiveProfileName() ?? Constants.DefaultProfileName;
                _configurations.Profiles[activeProfileName].Active = true;
                return GetActiveProfile();
            }

            if (activesProfile.Count() > 1)
                throw new Exception("More than 1 profile is active, please check your configuration file.");

            return activesProfile.FirstOrDefault();
        }

        public Dictionary<string, Profile> GetProfiles()
        {
            return _configurations.Profiles;
        }

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
            if (!_configurations.Profiles.ContainsKey(profileName))
                throw new Exception($"Profile name: {profileName} does not exists");


            _configurations.Profiles[activeProfile.Key].Active = false;
            _configurations.Profiles[profileName].Active = true;
            hostService.SetProfile(profileName, _configurations.Profiles[profileName]);
        }
        private HostHandler BuildHostService()
        {
            
            return new HostHandler(_configurations.HostsFileFolder);
        }
    }
}
