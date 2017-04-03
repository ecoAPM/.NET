using System;
using System.Threading.Tasks;
using CoreAPM.DotNet.Agent;
using CoreAPM.Events.Model;
using Microsoft.AspNetCore.Http;

namespace CoreAPM.DotNet.AspNetCoreMiddleware
{
    public class CoreAPMMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAgent _agent;
        private readonly ITimer _timer;

        public CoreAPMMiddleware(RequestDelegate next, IAgent agent, ITimer timer)
        {
            _timer = timer;
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
                Time = DateTime.Now,
                Length = await time
            };
            _agent.Send(e);
        }

        private async Task<double> getRequestTime(HttpContext httpContext)
        {
            _timer.Start();
            await _next.Invoke(httpContext);
            _timer.Stop();
            return _timer.CurrentTime;
        }
    }
}
