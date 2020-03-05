using System;

using System.IO;

using System.Text.Json;
using System.Text.Json.Serialization;
using HostsManager.Application.Configuration;

using Microsoft.Extensions.Configuration;

namespace HostsManager.Application
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
            .AddJsonFile("appsettings.json", optional:false,reloadOnChange:true)
            .Build();


        public Configurations GetConfigurations()
             => _configuration.Get<Configurations>();


        private void ResetConfigurations()
        {
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles\\appsettings.json";
            var filePathBackup = $"{AppDomain.CurrentDomain.BaseDirectory}ConfigurationFiles\\appsettings_BACKUP_{DateTime.Now:yyyy-MM-dd}.json";
            if(!File.Exists(filePathBackup))
                File.Move(filePath, filePathBackup);
            File.WriteAllText(filePath, JsonSerializer.Serialize(new Configurations()));

        }
        public void Teste()
        {
            //var configurations = GetConfigurations();
            //var hostsDirectory = new DirectoryInfo(configurations.HostsFileFolder);
            //var hostsService = new HostServices(hostsDirectory);
            //hostsService.SetProfile(configurations.Profiles.First());
            //hostsService.RollbackHostsFile();
            ResetConfigurations();

        }


    }

}
