using System;
using System.IO;
using System.Text.Json;
using HostsManager.Application.Configuration;
using Microsoft.Extensions.Configuration;

namespace HostsManager.Services
{
    public class ConfigurationsService
    {
        private readonly IConfiguration _configuration;
        public ConfigurationsService()
        {
            _configuration = InitializeConfiguration();
        }

        private IConfiguration InitializeConfiguration() => new ConfigurationBuilder()
            .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles")
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


        public Configurations GetConfigurations()
        {
            var configurations = _configuration.Get<Configurations>();
            return configurations;
        }


        private void ResetConfigurations()
        {
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles\\appsettings.json";
            var filePathBackup = $"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles\\appsettings_BACKUP_{DateTime.Now:yyyy-MM-dd}.json";
            if (!File.Exists(filePathBackup))
                File.Move(filePath, filePathBackup);
            File.WriteAllText(filePath, JsonSerializer.Serialize(new Configurations()));
        }

        public void SaveConfigurations(Configurations configurations)
        {
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles\\appsettings.json";
            var versionCounter = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles\\", "appsettings_v*").Length;
            var versionPath = $"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles\\appsettings_v{versionCounter+1}.json";
            if (!File.Exists(versionPath))
                File.Move(filePath, versionPath);
            File.WriteAllText(filePath, JsonSerializer.Serialize(configurations));
        }
    }

}
