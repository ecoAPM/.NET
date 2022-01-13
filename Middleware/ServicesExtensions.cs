using ecoAPM.Agent;
using Microsoft.Extensions.DependencyInjection;
using Timer = ecoAPM.Agent.Timer;

namespace ecoAPM.Middleware;

public static class ServicesExtensions
{
	public static void AddEcoAPM(this IServiceCollection services)
	{
		services.AddHttpClient<IAgent, QueuedAgent>();

		services.AddSingleton<Middleware>();
		services.AddSingleton<IAgent, QueuedAgent>();
		services.AddSingleton<IServerConfig, ServerConfig>();
		services.AddSingleton<Func<ITimer>>(_ => () => new Timer());
	}
}