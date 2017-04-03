using System.Net.Http;
using CoreAPM.DotNet.Agent;
using Microsoft.Extensions.DependencyInjection;

namespace CoreAPM.DotNet.ASPNETCoreMiddleware
{
    public static class CoreAPMServicesExtensions
    {
        public static void AddCoreAPM(this IServiceCollection services)
        {
            services.AddTransient<CoreAPMMiddleware>();
            services.AddTransient<HttpClient>();
            services.AddTransient<IAgent, QueuedAgent>();
            services.AddTransient<IConfig, Config>();
            services.AddTransient<ITimer, Timer>();
        }
    }
}