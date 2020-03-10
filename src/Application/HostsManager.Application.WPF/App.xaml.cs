using System;
using System.Windows;
using HostsManager.Services;
using HostsManager.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace HostsManager.Application.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        public IServiceProvider ServiceProvider { get; private set; }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddManagerService();
            services.AddSingleton<MainWindow>();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
