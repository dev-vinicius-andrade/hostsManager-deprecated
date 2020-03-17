using HostsManager.Services.Configuration;
using HostsManager.Services.Handlers;
using HostsManager.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace HostsManager.Services.Helpers
{
    public static class ServicesInjection
    {

        public static IConfiguration AddManagerService(this IServiceCollection services)
        {

            var configurationsHandler = new ConfigurationsHandler();
            services.Configure<HostsConfigurations>(configurationsHandler.Configuration, options => configurationsHandler.GetConfigurations());
            services
                .InjectService(configurationsHandler)
                .InjectService<IManagerService, ManagerService>(ServiceLifetime.Transient);
            return configurationsHandler.Configuration;
        }
        private static IServiceCollection InjectService<TService>(this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped) where TService : class
        {
            return lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton<TService>(),
                ServiceLifetime.Scoped => services.AddScoped<TService>(),
                ServiceLifetime.Transient => services.AddTransient<TService>(),
                _ => services
            };
        }
        private static IServiceCollection InjectService<TService, TImplementation>(this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class
            where TImplementation : class, TService
        {
            return lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton<TService, TImplementation>(),
                ServiceLifetime.Scoped => services.AddScoped<TService, TImplementation>(),
                ServiceLifetime.Transient => services.AddTransient<TService, TImplementation>(),
                _ => services
            };
        }
        private static IServiceCollection InjectService<TService>(this IServiceCollection services, TService service,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class
        {
            return lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton<TService>(s => service),
                ServiceLifetime.Scoped => services.AddScoped<TService>(s => service),
                ServiceLifetime.Transient => services.AddTransient<TService>(s => service),
                _ => services
            };
        }
    }
}
