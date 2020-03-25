using System.Linq;
using HostsManager.Services.Helpers;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace HostsManager.Application.Cli
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddManagerService();
            services.AddSingleton<ProfilesCommand>();
            var serviceProvider = services.BuildServiceProvider();
            var app = new CommandLineApplication();
            var commands = serviceProvider.GetServices<ProfilesCommand>().ToList();
            app.Commands.AddRange(commands);
            app.Execute(args);
        }
    }
}
