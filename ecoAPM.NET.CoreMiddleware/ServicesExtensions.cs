using ecoAPM.NET.Agent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Timer = ecoAPM.NET.Agent.Timer;

namespace ecoAPM.NET.CoreMiddleware;

public static class ServicesExtensions
{
	public static void AddEcoAPM(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddHttpClient<IAgent, QueuedAgent>();

		services.AddSingleton<Middleware>();
		services.AddSingleton<IAgent, QueuedAgent>();
		services.AddSingleton<IServerConfig>(_ => new ServerConfig(configuration));
		services.AddSingleton<Func<ITimer>>(_ => () => new Timer());
	}
}