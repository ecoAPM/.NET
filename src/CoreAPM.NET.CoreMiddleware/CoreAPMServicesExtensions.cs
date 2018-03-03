using System;
using System.Net.Http;
using CoreAPM.DotNet.Agent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreAPM.DotNet.AspNetCoreMiddleware
{
    public static class CoreAPMServicesExtensions
    {
        public static void AddCoreAPM(this IServiceCollection services, IConfiguration config = null)
        {
            services.AddTransient<CoreAPMMiddleware>();
            services.AddTransient<HttpClient>();
            services.AddTransient<IAgent, QueuedAgent>();
            services.AddTransient<IConfig, Config>();
            services.AddTransient<Func<ITimer>>(t => () => new Timer());

            if (config != null)
                services.AddTransient(c => config);
        }
    }
}