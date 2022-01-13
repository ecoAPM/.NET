using Microsoft.AspNetCore.Builder;

namespace ecoAPM.Middleware;

public static class MiddlewareExtensions
{
	/// <summary>Use ecoAPM's middleware to send metrics to the ecoAPM server</summary>
	/// <param name="app">The current application builder</param>
	/// <returns></returns>
	public static IApplicationBuilder UseEcoAPM(this IApplicationBuilder app)
		=> app.UseMiddleware<Middleware>();
}