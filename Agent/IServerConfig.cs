namespace ecoAPM.Agent;

/// <summary>A set of configuration values for the connection to the ecoAPM server</summary>
public interface IServerConfig
{
	/// <summary>The base URL of the ecoAPM server</summary>
	Uri BaseURL { get; }

	/// <summary>The API key that authorizes sending data</summary>
	Guid APIKey { get; }

	/// <summary>The interval to send data to the server at</summary>
	TimeSpan Interval { get; }
}