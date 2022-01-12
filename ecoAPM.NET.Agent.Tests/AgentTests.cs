using System.Text.Json;
using NSubstitute;
using Xunit;

namespace ecoAPM.NET.Agent.Tests;

public class AgentTests
{
	[Fact]
	public void RequestUrlCombinedCorrectlyWithSlashInBase()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
		var httpClient = Substitute.For<HttpClient>();

		//act
		var agent = new StubAgent(config, httpClient);

		//assert
		Assert.Equal("http://localhost/requests", agent.RequestUrl.AbsoluteUri);
	}

	[Fact]
	public void RequestUrlCombinedCorrectlyWithoutSlashInBase()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid());
		var httpClient = Substitute.For<HttpClient>();

		//act
		var agent = new StubAgent(config, httpClient);

		//assert
		Assert.Equal("http://localhost/requests", agent.RequestUrl.AbsoluteUri);
	}

	[Fact]
	public void APIKeyAddedToAuthHeader()
	{
		//arrange
		var apiKey = Guid.NewGuid();
		var config = new ServerConfig(new Uri("http://localhost/"), apiKey);
		var httpClient = Substitute.For<HttpClient>();

		//act
		var agent = new StubAgent(config, httpClient);

		//assert
		Assert.Contains(apiKey.ToString(), agent.HttpClient.DefaultRequestHeaders.Authorization?.ToString() ?? string.Empty);
	}

	[Fact]
	public void HttpClientIsDisposedOnDispose()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
		var httpClient = Substitute.For<HttpClient>();
		var agent = new StubAgent(config, httpClient);

		//act
		agent.Dispose();

		//assert
		httpClient.ReceivedWithAnyArgs().Dispose();
	}

	[Fact]
	public async Task SendPerformsHttpPost()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
		var http = new MockHttpMessageHandler();
		var agent = new Agent(config, new HttpClient(http));

		//act
		await agent.Send(new Request());

		//assert
		Assert.True(http.Posted);
	}

	[Fact]
	public async Task RequestCanConvertToJSON()
	{
		//arrange
		var request = new Request { Action = "a1" };

		//act
		var json = await Agent.GetPostContent(request).ReadAsStringAsync();

		//assert
		Assert.Equal("a1", JsonSerializer.Deserialize<Request>(json)!.Action);
	}
}