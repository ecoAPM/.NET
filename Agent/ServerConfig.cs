using Microsoft.Extensions.Configuration;

namespace ecoAPM.Agent;

/// <summary>A set of configuration values for the connection to the ecoAPM server</summary>
public class ServerConfig : IServerConfig
{
	public Uri BaseURL { get; }
	public string APIKey { get; }
	public TimeSpan Interval { get; }

	/// <summary>Configures the connection using environment variables</summary>
	public ServerConfig()
		: this(envBaseURL, envAPIKey, envInterval)
	{
	}

	/// <summary>Configures the connection using a pre-defined configuration</summary>
	/// <param name="config">The configuration to use</param>
	public ServerConfig(IConfiguration config)
		: this(config["ecoAPM:BaseURL"] ?? config["ecoAPM_BaseURL"],
			config["ecoAPM:APIKey"] ?? config["ecoAPM_APIKey"],
			config["ecoAPM:Interval"] ?? config["ecoAPM_Interval"])
	{
	}

	/// <summary>Configures the connection using explicit values</summary>
	/// <param name="baseURL">The base URL of the ecoAPM server</param>
	/// <param name="apiKey">The API key that authorizes sending data</param>
	/// <param name="interval">The interval to send data to the server at</param>
	public ServerConfig(string? baseURL, string? apiKey, string? interval = null)
		: this(
			GetBaseURL(baseURL ?? envBaseURL),
			GetAPIKey(apiKey ?? envAPIKey),
			GetInterval(interval ?? envInterval)
		)
	{
	}

	/// <summary>Configures the connection using strongly-typed values</summary>
	/// <param name="baseURL">The base URL of the ecoAPM server</param>
	/// <param name="apiKey">The API key that authorizes sending data</param>
	/// <param name="interval">The interval to send data to the server at</param>
	public ServerConfig(Uri? baseURL, string? apiKey, TimeSpan? interval = null)
	{
		BaseURL = baseURL ?? GetBaseURL(envBaseURL);
		APIKey = apiKey ?? GetAPIKey(envAPIKey);
		Interval = interval ?? GetInterval(envInterval) ?? DefaultInterval;
	}

	private static Uri GetBaseURL(string baseURL)
		=> Uri.TryCreate(baseURL, UriKind.Absolute, out var baseURI)
			? baseURI
			: throw new ArgumentNullException(nameof(baseURL), "ecoAPM base URL not set!");

	private static string GetAPIKey(string? apiKey)
		=> apiKey
		   ?? throw new ArgumentNullException(nameof(apiKey), "ecoAPM API Key not set!");

	private static TimeSpan? GetInterval(string seconds)
		=> ushort.TryParse(seconds, out var num)
			? TimeSpan.FromSeconds(num)
			: null;

	private static string envBaseURL => Environment.GetEnvironmentVariable("ecoAPM_BaseURL") ?? string.Empty;
	private static string envAPIKey => Environment.GetEnvironmentVariable("ecoAPM_APIKey") ?? string.Empty;
	private static string envInterval => Environment.GetEnvironmentVariable("ecoAPM_Interval") ?? string.Empty;

	private static readonly TimeSpan DefaultInterval = TimeSpan.FromSeconds(5);
}