using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ecoAPM.NET.Agent.Tests;

public class QueuedAgentTests
{
	[Fact]
	public async Task SendAddsRequestToQueue()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid(), TimeSpan.MaxValue);
		var httpClient = Substitute.For<HttpClient>();
		var agent = new QueuedAgent(config, httpClient);
		var request = new Request();

		//act
		await agent.Send(request);

		//assert
		Assert.Contains(request, agent.GetRequestsToSend());
	}

	[Fact]
	public async Task RequestQueueIsClearedOnDispose()
	{
		var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid(), TimeSpan.MaxValue);
		var http = new MockHttpMessageHandler();
		var httpClient = new HttpClient(http);
		var agent = new QueuedAgent(config, httpClient);
		await agent.Send(new Request());

		//act
		agent.Dispose();

		//assert
		var leftover = agent.GetRequestsToSend();
		Assert.Empty(leftover);
	}

	[Fact]
	public async Task PostContentContainsAllRequests()
	{
		//arrange
		var requests = new[] { new Request { Action = "a1" }, new Request { Action = "a2" } };

		//act
		var json = await QueuedAgent.GetPostContent(requests).ReadAsStringAsync();

		//assert
		var actions = JsonSerializer.Deserialize<IEnumerable<Request>>(json)!.Select(t => t.Action).ToList();
		Assert.Contains("a1", actions);
		Assert.Contains("a2", actions);
	}

	[Fact]
	public async Task SendRequestsCallsHttpPost()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid(), TimeSpan.MaxValue);
		var http = new MockHttpMessageHandler();
		var agent = new QueuedAgent(config, new HttpClient(http));
		var requests = new[] { new Request { Action = "a1" }, new Request { Action = "a2" } };

		//act
		await agent.SendRequests(requests);

		//assert
		Assert.True(http.Posted);
	}

	[Fact]
	public async Task SendRemovesSentRequestsFromQueue()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid(), TimeSpan.MaxValue);
		var http = new MockHttpMessageHandler();
		var httpClient = new HttpClient(http);
		var agent = new QueuedAgent(config, httpClient);
		var r1 = new Request { Action = "a1" };
		var r2 = new Request { Action = "a2" };
		var r3 = new Request { Action = "a3" };
		await agent.Send(r1);
		await agent.Send(r2);
		await agent.Send(r3);

		//act
		var toSend = new[] { r1, r2 };
		await agent.SendRequests(toSend);

		//assert
		var remaining = agent.GetRequestsToSend();
		Assert.DoesNotContain(r1, remaining);
		Assert.DoesNotContain(r2, remaining);
		Assert.Contains(r3, remaining);
	}

	[Fact]
	public void SenderStartedOnConstruction()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid(), TimeSpan.MaxValue);
		var httpClient = Substitute.For<HttpClient>();

		//act
		var agent = new QueuedAgent(config, httpClient);

		//assert
		Assert.True(agent.IsRunning);
	}

	[Fact]
	public void SenderStopsOnDispose()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid(), TimeSpan.Zero);
		var httpClient = Substitute.For<HttpClient>();
		var agent = new QueuedAgent(config, httpClient);
		agent.Send(new Request());

		//act
		agent.Dispose();

		//assert
		Assert.False(agent.IsRunning);
	}

	[Fact]
	public async Task ErrorLogsWarning()
	{
		//arrange
		var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid(), TimeSpan.MaxValue);
		var http = new MockHttpMessageHandler(HttpStatusCode.BadRequest);
		var loggerFactory = Substitute.For<ILoggerFactory>();
		var logger = new MockLogger();
		loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);
		var agent = new QueuedAgent(config, new HttpClient(http), loggerFactory);

		//act
		await agent.SendRequests(new [] { new Request() });

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
		var agent = new QueuedAgent(config, new HttpClient(http), loggerFactory);

		//act
		await agent.SendRequests(new [] { new Request() });

		//assert
		var log = logger.Output.ToString();
		Assert.DoesNotContain("Warning:", log);
	}
}