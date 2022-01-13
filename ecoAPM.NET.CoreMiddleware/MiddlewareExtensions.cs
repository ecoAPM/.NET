using Microsoft.AspNetCore.Builder;

namespace ecoAPM.NET.CoreMiddleware;

public static class MiddlewareExtensions
{
	public static IApplicationBuilder UseEcoAPM(this IApplicationBuilder app)
		=> app.UseMiddleware<Middleware>();
}