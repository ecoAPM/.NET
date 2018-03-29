using System;
using System.Net.Http;
using CoreAPM.NET.Agent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreAPM.NET.CoreMiddleware
{
    public static class CoreAPMServicesExtensions
    {
        public static void AddCoreAPM(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient<CoreAPMMiddleware>();
            services.TryAddTransient<HttpClient>();
            services.TryAddTransient<IAgent, QueuedAgent>();
            services.TryAddTransient<IServerConfig>(sp => new ServerConfig(configuration));
            services.TryAddTransient<Func<ITimer>>(t => () => new Timer());
        }
    }
}