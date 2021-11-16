using ecoAPM.NET.Agent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Timer = ecoAPM.NET.Agent.Timer;

namespace ecoAPM.NET.CoreMiddleware;

public static class ecoAPMServicesExtensions
{
	public static void AddecoAPM(this IServiceCollection services, IConfiguration configuration)
	{
		services.TryAddTransient<ecoAPMMiddleware>();
		services.TryAddTransient<HttpClient>();
		services.TryAddTransient<IAgent, QueuedAgent>();
		services.TryAddTransient<IServerConfig>(_ => new ServerConfig(configuration));
		services.TryAddTransient<Func<ITimer>>(_ => () => new Timer());
	}
}