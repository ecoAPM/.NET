namespace ecoAPM.NET.Agent.Tests;

public class StubAgent : Agent
{
	public Uri RequestUrl => _requestURL;
	public HttpClient HttpClient => _httpClient;

	public StubAgent(ServerConfig config, HttpClient httpClient) : base(config, httpClient)
	{
	}

	public override Task Send(Request request) => throw new NotImplementedException();
}