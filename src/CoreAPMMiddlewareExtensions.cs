using CoreAPM.DotNet.Agent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace CoreAPM.DotNet.ASPNETCoreMiddleware
{
    public static class CoreAPMMiddlewareExtensions
    {
        public static IApplicationBuilder UseCoreAPM(this IApplicationBuilder app) => app.UseMiddleware<CoreAPMMiddleware>();
        public static IApplicationBuilder UseCoreAPM(this IApplicationBuilder app, IConfiguration config) => app.UseMiddleware<CoreAPMMiddleware>(new Config(config));
    }
}