using Microsoft.AspNetCore.Builder;

namespace CoreAPM.NET.CoreMiddleware
{
    public static class CoreAPMMiddlewareExtensions
    {
        public static IApplicationBuilder UseCoreAPM(this IApplicationBuilder app) => app.UseMiddleware<CoreAPMMiddleware>();
    }
}