using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HostsManager.Services.Entities;
using HostsManager.Services.Helpers;

namespace HostsManager.Services.Handlers
{
    internal class HostHandler 
    {
        private readonly DirectoryInfo _hostsDirectory;
        private readonly List<Hosts> _defaultHosts;
        private const string HostsFileName = "HOSTS";
        private const string OriginalHostsFileName = "HOSTS_ORIGINAL";
        private const string ActiveProfileIdentifier = "##ACTIVE_PROFILE:";
        private string HostsFilePath => $"{_hostsDirectory.FullName}\\{HostsFileName}";
        private string OriginalHostsFilePath => $"{_hostsDirectory.FullName}\\{OriginalHostsFileName}";
        public HostHandler(string hostsDirectory)
        {
            _hostsDirectory = PersistHostsDirectory(hostsDirectory);
            _defaultHosts = ReadDefaultHosts();
        }

        private DirectoryInfo PersistHostsDirectory(string directory)
        {
            var folder = new DirectoryInfo(directory);
            return folder.Exists ? folder :
                throw new Exception($"Invalid Hosts File Folder, please check your configuration. {directory}");

        }
        public void SetProfile(string profileName, Profile profile)
        {
            CreateNewHostsFile();
            var fileText = new List<string> {$"{ActiveProfileIdentifier}{profileName}", "###   ACTIVE HOSTS"};
            fileText.AddRange(profile.Hosts.Where(host => host.Active).Select(host => $"{host.Ip}    {host.Host}"));
            fileText.Add("###   INACTIVE HOSTS");
            fileText.AddRange(profile.Hosts.Where(host => !host.Active).Select(host => $"#{host.Ip}    {host.Host}"));
            fileText.Add(string.Empty);
            fileText.Add(string.Empty);
            fileText.Add(string.Empty);
            fileText.Add(string.Empty);
            fileText.Add(string.Empty);
            fileText.Add($"###  DEFAULT_HOSTS");
            fileText.AddRange(_defaultHosts.Select(host => $"###{host.Ip}    {host.Host}"));
            File.WriteAllLines(HostsFilePath, fileText);
        }
        public void RollbackHostsFile()
        {
            if (!File.Exists(HostsFilePath) || !File.Exists(OriginalHostsFilePath))
                throw new Exception("It's not possible to rollback in the actual state");
            File.Delete(HostsFilePath);
            File.Move(OriginalHostsFilePath, HostsFilePath);
        }

        private List<Hosts> ReadDefaultHosts()
        {
            var filePath = File.Exists(OriginalHostsFilePath) ? OriginalHostsFilePath : HostsFilePath;
            var fileTextList = File.ReadAllLines(filePath).ToList();

            var uncommentedTextList = fileTextList.Where(p => !p.StartsWith("#")).ToList();

            var defaultHosts = new List<Hosts>();
            foreach (var uncommentedText in uncommentedTextList)
            {
                if (uncommentedText.Length <= 0) continue;
                var hostInfo = uncommentedText.TrimStart().TrimEnd().SplitByEmptySpace();
                defaultHosts.Add(new Hosts
                {
                    Ip = hostInfo[0],
                    Host = hostInfo[1],
                    Active = true
                });
            }

            return defaultHosts;
        }

        public string GetActiveProfileName()
        {
            var headerLine = File.ReadLines(HostsFilePath).First();
            if (headerLine.Contains(ActiveProfileIdentifier))
                return headerLine.Split(ActiveProfileIdentifier)[1];
            return null;

        }

        private void CreateNewHostsFile()
        {
            if (!File.Exists(OriginalHostsFilePath))
                File.Move(HostsFilePath, OriginalHostsFilePath);
            File.Create(HostsFilePath).Close();
        }



        public IReadOnlyList<Hosts> DefaultHosts() => _defaultHosts;




    }
}
