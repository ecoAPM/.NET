using Microsoft.AspNetCore.Builder;

namespace CoreAPM.DotNet.AspNetCoreMiddleware
{
    public static class CoreAPMMiddlewareExtensions
    {
        public static IApplicationBuilder UseCoreAPM(this IApplicationBuilder app) => app.UseMiddleware<CoreAPMMiddleware>();
    }
}