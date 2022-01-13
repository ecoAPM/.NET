using ecoAPM.Agent;
using Microsoft.AspNetCore.Http;

namespace ecoAPM.Middleware;

/// <summary>Automatically sends request data to the configured ecoAPM server</summary>
public class Middleware
{
	private readonly RequestDelegate _next;
	private readonly IAgent _agent;
	private readonly Func<ITimer> _newTimer;

	/// <summary>Creates a new ecoAPM middleware object</summary>
	/// <param name="next">The next middleware object to run</param>
	/// <param name="agent">The agent responsible for sending requests</param>
	/// <param name="timerFac">A factory method that creates a new timer</param>
	public Middleware(RequestDelegate next, IAgent agent, Func<ITimer> timerFac)
	{
		_next = next;
		_agent = agent;
		_newTimer = timerFac;
	}

	/// <summary>Times the current HTTP request and sends metrics to the ecoAPM server</summary>
	/// <param name="httpContext">The context of the current HTTP request</param>
	public async Task Invoke(HttpContext httpContext)
	{
		var start = DateTime.UtcNow;
		var time = await GetResponseTime(httpContext);
		var request = CreateRequest(httpContext, start, time);
		_ = _agent.Send(request);
	}

	private async Task<double> GetResponseTime(HttpContext httpContext)
	{
		var timer = _newTimer();
		timer.Start();
		await _next.Invoke(httpContext);
		timer.Stop();
		return timer.CurrentTime;
	}

	private static Request CreateRequest(HttpContext httpContext, DateTime start, double time)
		=> new()
		{
			ID = Guid.NewGuid(),
			Type = "ServerResponse",
			Source = httpContext.Request.Host.Value,
			Action = httpContext.Request.Path.Value,
			Result = httpContext.Response.StatusCode.ToString(),
			Context = httpContext.TraceIdentifier,
			Time = start,
			Length = time
		};
}