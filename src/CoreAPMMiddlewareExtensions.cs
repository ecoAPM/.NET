using CoreAPM.DotNet.Agent;
using Microsoft.AspNetCore.Builder;

namespace CoreAPM.DotNet.ASPNETCoreMiddleware
{
    public static class CoreAPMMiddlewareExtensions
    {
        public static IApplicationBuilder UseCoreAPM(this IApplicationBuilder app) => app.UseMiddleware<CoreAPMMiddleware>();
        public static IApplicationBuilder UseCoreAPM(this IApplicationBuilder app, Config config) => app.UseMiddleware<CoreAPMMiddleware>(config);
    }
}