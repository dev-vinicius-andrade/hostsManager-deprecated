using System;
using System.Windows;
using HostsManager.Application.WPF.Configuration;
using HostsManager.Application.WPF.Controller;
using HostsManager.Services.Helpers;
using Microsoft.Extensions.Configuration;
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
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            _iconTrayController = ServiceProvider.GetRequiredService<IconTrayController>();
        }
        
        public IServiceProvider ServiceProvider { get; private set; }
        private void ConfigureServices(IServiceCollection services)
        {
            var configuration =  services.AddManagerService();
            var uiConfigurations = configuration.Get<UiConfigurations>();
            services.AddSingleton(uiConfigurations);
            services.AddSingleton(uiConfigurations.ThemeConfiguration);
            services.AddSingleton(uiConfigurations.MainWindowConfigurations);
            services.AddSingleton(uiConfigurations.ProfileWindowConfigurations);
            services.AddSingleton<ThemeController>();
            services.AddTransient<MainWindow>();
            services.AddScoped<IDisposable>(p=>this);
            services.AddSingleton<IconTrayController>();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            if (OsHelper.IsWindows())
                _iconTrayController.Configure(mainWindow);
        }
        public void Dispose()
        {
            MainWindow?.Close();
        }
    }
}
