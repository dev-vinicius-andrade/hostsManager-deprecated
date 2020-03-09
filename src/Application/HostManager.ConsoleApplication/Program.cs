using System;
using HostsManager.Services;

namespace HostManager.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            new ConfigurationsService().Teste();
        }
    }
}
