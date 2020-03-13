using System;
using System.Windows;
using HostsManager.Application.WPF.Controller;
using HostsManager.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace HostsManager.Application.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App :IDisposable
    {
        private readonly IconTrayController _iconTrayController;


        public App()
        {
            _iconTrayController = new IconTrayController(this);
        }
        
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
            if (OsHelper.IsWindows())
                _iconTrayController.Configure(mainWindow);
        }
        public void Dispose()
        {
            MainWindow?.Close();
        }
    }
}
