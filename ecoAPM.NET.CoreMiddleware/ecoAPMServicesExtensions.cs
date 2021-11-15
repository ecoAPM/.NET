using System;
using System.Net.Http;
using ecoAPM.NET.Agent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ecoAPM.NET.CoreMiddleware;

public static class ecoAPMServicesExtensions
{
	public static void AddecoAPM(this IServiceCollection services, IConfiguration configuration)
	{
		services.TryAddTransient<ecoAPMMiddleware>();
		services.TryAddTransient<HttpClient>();
		services.TryAddTransient<IAgent, QueuedAgent>();
		services.TryAddTransient<IServerConfig>(sp => new ServerConfig(configuration));
		services.TryAddTransient<Func<ITimer>>(t => () => new Timer());
	}
}
