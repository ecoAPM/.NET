using System;
using System.Threading.Tasks;
using CoreAPM.NET.Agent;
using Microsoft.AspNetCore.Http;

namespace CoreAPM.NET.CoreMiddleware
{
    public class CoreAPMMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAgent _agent;
        private readonly Func<ITimer> newTimer;

        public CoreAPMMiddleware(RequestDelegate next, IAgent agent, Func<ITimer> timerFac)
        {
            newTimer = timerFac;
            _next = next;
            _agent = agent;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var time = getRequestTime(httpContext);
            var e = new Event
            {
                ID = Guid.NewGuid(),
                Type = "ServerResponse",
                Source = httpContext.Request.Host.Value,
                Action = httpContext.Request.Path.Value,
                Result = httpContext.Response.StatusCode.ToString(),
                Context = httpContext.TraceIdentifier,
                Time = DateTime.Now,
                Length = await time
            };
            _agent.Send(e);
        }

        private async Task<double> getRequestTime(HttpContext httpContext)
        {
            var timer = newTimer();
            timer.Start();
            await _next.Invoke(httpContext);
            timer.Stop();
            return timer.CurrentTime;
        }
    }
}
