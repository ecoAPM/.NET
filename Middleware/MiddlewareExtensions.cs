using Microsoft.AspNetCore.Builder;

namespace ecoAPM.Middleware;

public static class MiddlewareExtensions
{
	public static IApplicationBuilder UseEcoAPM(this IApplicationBuilder app)
		=> app.UseMiddleware<Middleware>();
}