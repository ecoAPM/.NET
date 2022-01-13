namespace ecoAPM.Agent;

/// <summary>Sends requests to an ecoAPM server</summary>
public interface IAgent
{
	/// <summary>Sends a request to the configured ecoAPM server</summary>
	/// <param name="request">The requests to send</param>
	Task Send(Request request);
}