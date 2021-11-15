using Microsoft.AspNetCore.Builder;

namespace ecoAPM.NET.CoreMiddleware;

public static class ecoAPMMiddlewareExtensions
{
	public static IApplicationBuilder UseecoAPM(this IApplicationBuilder app) => app.UseMiddleware<ecoAPMMiddleware>();
}
