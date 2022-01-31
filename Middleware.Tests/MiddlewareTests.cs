using ecoAPM.Agent;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace ecoAPM.Middleware.Tests;

public class MiddlewareTests
{
	[Fact]
	public async Task SendsRequestWhenInvoked()
	{
		//arrange
		var next = Substitute.For<RequestDelegate>();
		var timer = Substitute.For<ITimer>();
		var agent = Substitute.For<IAgent>();
		var context = Substitute.For<HttpContext>();
		var middleware = new Middleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		await agent.Received().Send(Arg.Any<Request>());
	}

	[Fact]
	public async Task InvokesNextWhenInvoked()
	{
		//arrange
		var next = Substitute.For<RequestDelegate>();
		var timer = Substitute.For<ITimer>();
		var agent = Substitute.For<IAgent>();
		var context = Substitute.For<HttpContext>();
		var middleware = new Middleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		await next.Received().Invoke(context);
	}

	[Fact]
	public async Task ReportsDurationFromTimer()
	{
		//arrange
		var next = Substitute.For<RequestDelegate>();
		var timer = Substitute.For<ITimer>();
		var agent = Substitute.For<IAgent>();
		var context = Substitute.For<HttpContext>();
		var duration = 0.0;
		timer.CurrentTime.Returns(123);
		agent.When(a => a.Send(Arg.Any<Request>())).Do(c => duration = ((Request)c.Args().First()).Duration);
		var middleware = new Middleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		Assert.Equal(123, duration);
	}

	[Fact]
	public async Task ReportsTimeInUTC()
	{
		//arrange
		var next = Substitute.For<RequestDelegate>();
		var timer = Substitute.For<ITimer>();
		var agent = Substitute.For<IAgent>();
		var context = Substitute.For<HttpContext>();
		var time = new DateTime();
		timer.CurrentTime.Returns(123);
		agent.When(a => a.Send(Arg.Any<Request>())).Do(c => time = ((Request)c.Args().First()).Time);
		var middleware = new Middleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		Assert.Equal(time.ToUniversalTime(), time);
	}
}