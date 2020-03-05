using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HostsManager.Application.Entities;
using HostsManager.Application.Helpers;
using HostsManager.Application.Interfaces;

namespace HostsManager.Application.Services
{
    internal class HostServices:IHostService
    {
        private readonly DirectoryInfo _hostsDirectory;
        private readonly List<Hosts> _defaultHosts;
        private const string HostsFileName = "HOSTS";
        private const string OriginalHostsFileName = "HOSTS_ORIGINAL";

        public HostServices(DirectoryInfo hostsDirectory)
        {
            _hostsDirectory = hostsDirectory;
            _defaultHosts= ReadDefaultHosts();
        }

        private List<Hosts> ReadDefaultHosts()
        {
            var filePath = File.Exists(OriginalHostsFilePath) ? OriginalHostsFilePath : HostsFilePath;
            var fileTextList = File.ReadAllLines(filePath).ToList();
            
            var uncommentedTextList = fileTextList.Where(p => !p.StartsWith("#"));

            var defaultHosts = new List<Hosts>();
            foreach (var uncommentedText in uncommentedTextList)
            {
                if (uncommentedText.Length <= 0) continue;
                var hostInfo = uncommentedText.TrimStart().TrimEnd().SplitByEmptySpace();
                defaultHosts.Add(new Hosts
                {
                    Ip = hostInfo[0],
                    Host = hostInfo[1]
                });
            }

            return defaultHosts;
        }
        public void CreateNewHostsFile()
        {
            File.Move(HostsFilePath, OriginalHostsFilePath);
        }

        public void SetProfile(string profileName, Profile profile)
        {
            CreateNewHostsFile();
            var fileText = new List<string> {$"##ACTIVE_PROFILE => {profileName}"};
            fileText.AddRange(profile.Hosts.Select(host => $"{host.Ip}    {host.Host}"));
            fileText.Add($"#DEFAULT_HOSTS");
            fileText.AddRange(_defaultHosts.Select(host => $"##{host.Ip}    {host.Host}"));
            File.WriteAllLines(HostsFilePath,fileText);
        }

        public IReadOnlyList<Hosts> DefaultHosts() => _defaultHosts;
        

        public void RollbackHostsFile()
        {
            if(!File.Exists(HostsFilePath) || !File.Exists(OriginalHostsFilePath))
                throw  new Exception("It's not possible to rollback in the actual state");
            File.Delete(HostsFilePath);
            File.Move(OriginalHostsFilePath,HostsFilePath);
        }

        private string HostsFilePath => $"{_hostsDirectory.FullName}\\{HostsFileName}";
        private string OriginalHostsFilePath => $"{_hostsDirectory.FullName}\\{OriginalHostsFileName}";
    }
}
