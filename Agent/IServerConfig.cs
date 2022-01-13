namespace ecoAPM.Agent;

public interface IServerConfig
{
	Uri BaseURL { get; }
	Guid APIKey { get; }
	TimeSpan Interval { get; }
}