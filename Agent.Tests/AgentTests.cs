using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ecoAPM.Agent.Tests;

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
		var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey.ToString()));
		Assert.Contains(base64, agent.HttpClient.DefaultRequestHeaders.Authorization?.ToString() ?? string.Empty);
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
		Assert.NotNull(http.Posted);
	}

	[Fact]
	public async Task RequestCanConvertToJSON()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
		var http = new MockHttpMessageHandler();
		var agent = new Agent(config, new HttpClient(http));

		//act
		var request = new Request { Action = "a1" };
		await agent.Send(request);

		//assert
		Assert.Equal("a1", JsonSerializer.Deserialize<Request>(await http.Posted!.ReadAsStringAsync())!.Action);
	}

	[Fact]
	public async Task ErrorLogsWarning()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
		var http = new MockHttpMessageHandler(HttpStatusCode.BadRequest);
		var loggerFactory = Substitute.For<ILoggerFactory>();
		var logger = new MockLogger();
		loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);
		var agent = new Agent(config, new HttpClient(http), loggerFactory);

		//act
		await agent.Send(new Request());

		//assert
		var log = logger.Output.ToString();
		Assert.Contains("Warning:", log);
	}

	[Fact]
	public async Task SuccessDoesNotLogWarning()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
		var http = new MockHttpMessageHandler();
		var loggerFactory = Substitute.For<ILoggerFactory>();
		var logger = new MockLogger();
		loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);
		var agent = new Agent(config, new HttpClient(http), loggerFactory);

		//act
		await agent.Send(new Request());

		//assert
		var log = logger.Output.ToString();
		Assert.DoesNotContain("Warning:", log);
	}
}