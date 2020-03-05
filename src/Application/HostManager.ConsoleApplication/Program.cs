using System;
using HostsManager.Application;

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
