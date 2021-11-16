using ecoAPM.NET.Agent;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace ecoAPM.NET.CoreMiddleware.Tests;

public class ecoAPMMiddlewareTests
{
	[Fact]
	public async Task SendsEventWhenInvoked()
	{
		//arrange
		var next = Substitute.For<RequestDelegate>();
		var timer = Substitute.For<ITimer>();
		var agent = Substitute.For<IAgent>();
		var context = Substitute.For<HttpContext>();
		var middleware = new ecoAPMMiddleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		await agent.Received().Send(Arg.Any<Event>());
	}

	[Fact]
	public async Task InvokesNextWhenInvoked()
	{
		//arrange
		var next = Substitute.For<RequestDelegate>();
		var timer = Substitute.For<ITimer>();
		var agent = Substitute.For<IAgent>();
		var context = Substitute.For<HttpContext>();
		var middleware = new ecoAPMMiddleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		await next.Received().Invoke(context);
	}

	[Fact]
	public async Task ReportsLengthFromTimer()
	{
		//arrange
		var next = Substitute.For<RequestDelegate>();
		var timer = Substitute.For<ITimer>();
		var agent = Substitute.For<IAgent>();
		var context = Substitute.For<HttpContext>();
		var length = 0.0;
		timer.CurrentTime.Returns(123);
		agent.When(a => a.Send(Arg.Any<Event>())).Do(c => length = ((Event)c.Args().First()).Length);
		var middleware = new ecoAPMMiddleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		Assert.Equal(123, length);
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
		agent.When(a => a.Send(Arg.Any<Event>())).Do(c => time = ((Event)c.Args().First()).Time);
		var middleware = new ecoAPMMiddleware(next, agent, () => timer);

		//act
		await middleware.Invoke(context);

		//assert
		Assert.Equal(time.ToUniversalTime(), time);
	}
}