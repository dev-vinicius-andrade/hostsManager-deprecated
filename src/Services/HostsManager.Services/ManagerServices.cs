using HostsManager.Application.Configuration;
using HostsManager.Application.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HostsManager.Services
{
    public class ManagerServices
    {
        private readonly Configurations _configurations;
        public ManagerServices()
        {
            _configurations = GetConfigurations();
            AddDefaultConfiguration();
        }

        private void AddDefaultConfiguration()
        {
            var defaultConfiguration = GetDefaultConfiguration();
            
            _configurations.Profiles.Add(defaultConfiguration.Key, defaultConfiguration.Value);
        }
        private Configurations GetConfigurations()
        {
            try
            {
                var service = new ConfigurationsService();
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

            var activesProfile = _configurations.Profiles.Where(p => p.Value.Active);
            if (!activesProfile.Any())
            {
                var hostService = BuildHostService();
                var activeProfileName = hostService.GetActiveProfileName();
                if (activeProfileName == null)
                    activeProfileName = Constants.DEFAULT_PROFILE_NAME;
                _configurations.Profiles[activeProfileName].Active = true;
                return GetActiveProfile();
            }

            if (activesProfile.Count() > 1)
                throw new Exception("More than 1 profile is active, please check your configuration file.");

            return activesProfile.FirstOrDefault();
        }

        public Dictionary<string, Profile> GetProfiles()
        {
            var profiles = new Dictionary<string, Profile>();
            foreach (var profile in _configurations.Profiles)
                profiles.Add(profile.Key, profile.Value);
            return profiles;

        }

        public KeyValuePair<string, Profile> GetDefaultConfiguration()
        {
            var hostService = BuildHostService();
            var defaultHosts = hostService.DefaultHosts();
            return new KeyValuePair<string, Profile>(
                key: Constants.DEFAULT_PROFILE_NAME,
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
        private HostServices BuildHostService()
        {
            var hostsFileFoder = new DirectoryInfo(_configurations.HostsFileFolder);
            if (!hostsFileFoder.Exists)
                throw new Exception($"Invalid Hosts File Folder, please check your configuration. {_configurations.HostsFileFolder}");
            return new HostServices(hostsFileFoder);
        }
    }
}
