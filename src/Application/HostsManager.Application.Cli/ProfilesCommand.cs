using System;
using HostsManager.Services.Interfaces;
using Microsoft.Extensions.CommandLineUtils;

namespace HostsManager.Application.Cli
{
    internal class ProfilesCommand: CommandLineApplication
    {
        private readonly IManagerService _managerService;

        public ProfilesCommand(IManagerService managerService)
        {
            _managerService = managerService;
            Name = "-profiles";
            MapCommands();
        }

        private void MapCommands()
        {
            Command("-list", application => application.OnExecute(ListProfiles));
        }

        private int ListProfiles()
        {
            var profiles = _managerService.GetProfiles();
            foreach (var profile in profiles)
                Console.WriteLine(profile.Key);
            return profiles.Count;
        }
    }
}
