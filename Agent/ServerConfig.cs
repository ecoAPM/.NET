using Microsoft.Extensions.Configuration;

namespace ecoAPM.Agent;

public class ServerConfig : IServerConfig
{
	public Uri BaseURL { get; }
	public Guid APIKey { get; }
	public TimeSpan Interval { get; }

	public ServerConfig()
		: this(envBaseURL, envAPIKey, envInterval)
	{
	}

	public ServerConfig(IConfiguration config)
		: this(config["ecoAPM:BaseURL"] ?? config["ecoAPM_BaseURL"] ?? envBaseURL,
			config["ecoAPM:APIKey"] ?? config["ecoAPM_APIKey"] ?? envAPIKey,
			config["ecoAPM:Interval"] ?? config["ecoAPM_Interval"] ?? envInterval)
	{
	}

	public ServerConfig(string? baseURL, string? apiKey, string? interval = null)
		: this(new Uri(baseURL ?? envBaseURL), new Guid(apiKey ?? envAPIKey), GetInterval(interval ?? envInterval))
	{
	}

	public ServerConfig(Uri? baseURL, Guid? apiKey, TimeSpan? interval = null)
	{
		BaseURL = baseURL ?? new Uri(envBaseURL);
		APIKey = apiKey ?? new Guid(envAPIKey);
		Interval = interval ?? GetInterval(envInterval) ?? DefaultInterval;
	}

	private static TimeSpan? GetInterval(string seconds)
	{
		return ushort.TryParse(seconds, out var num)
			? TimeSpan.FromSeconds(num)
			: null;
	}

	private static string envBaseURL => Environment.GetEnvironmentVariable("ecoAPM_BaseURL") ?? string.Empty;
	private static string envAPIKey => Environment.GetEnvironmentVariable("ecoAPM_APIKey") ?? string.Empty;
	private static string envInterval => Environment.GetEnvironmentVariable("ecoAPM_Interval") ?? string.Empty;

	private static readonly TimeSpan DefaultInterval = TimeSpan.FromSeconds(5);
}