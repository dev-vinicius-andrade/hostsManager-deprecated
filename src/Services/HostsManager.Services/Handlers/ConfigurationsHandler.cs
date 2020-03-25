using System;
using System.IO;
using System.Text.Json;
using HostsManager.Services.Configuration;
using Microsoft.Extensions.Configuration;

namespace HostsManager.Services.Handlers
{
    internal class ConfigurationsHandler
    {
        public readonly IConfiguration Configuration;

        private readonly string _configurationFileFolder;
        private readonly string _variableFilename;
        private readonly string _configurationFilePath;
        public ConfigurationsHandler(IConfiguration configuration = null)
        {
            _variableFilename = "hostsConfigurations{0}.json";
            _configurationFileFolder = $"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles";
            _configurationFilePath = $"{_configurationFileFolder}\\{DefaultFileName}";
            Configuration = configuration ?? InitializeConfiguration();
        }

        private string DefaultFileName => string.Format(_variableFilename, string.Empty);
        private string BackupFileName => string.Format(_variableFilename, $"_BACKUP_{DateTime.Now:yyyy-MM-dd_HHmmss}");
        private IConfiguration InitializeConfiguration(IConfiguration configuration = null)
        {
            var builder = new ConfigurationBuilder();
            if (configuration != null)
                builder.AddConfiguration(configuration);
            return builder.SetBasePath(_configurationFileFolder)
                .AddJsonFile("uiConfigurations.json", optional:true)
                .AddJsonFile(DefaultFileName, optional: false, reloadOnChange: true)
                .Build();
        }

        public HostsConfigurations GetConfigurations()
        => Configuration.Get<HostsConfigurations>();
         



        public void ResetConfigurations()
        {
            var backupFilePath = $"{_configurationFileFolder}\\{BackupFileName}";
            if (!File.Exists(backupFilePath))
                File.Move(_configurationFilePath, backupFilePath);
            File.WriteAllText(_configurationFilePath, JsonSerializer.Serialize(new HostsConfigurations()));
        }

        public void SaveConfigurations(HostsConfigurations hostsConfigurations)
        {
            var versionCounter = Directory.GetFiles(_configurationFileFolder, "appsettings_v*").Length;
            var versionPath = $"{_configurationFileFolder}{string.Format(_variableFilename, "_v"+versionCounter + 1)}";
            if (!File.Exists(versionPath))
                File.Move(_configurationFilePath, versionPath);
            File.WriteAllText(_configurationFilePath, JsonSerializer.Serialize(hostsConfigurations));
        }

        public string GetConfigurationsFileFolder()
        {
            return _configurationFileFolder;
        }
    }

}
