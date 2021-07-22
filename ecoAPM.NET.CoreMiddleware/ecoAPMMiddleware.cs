﻿using System;
using System.Threading.Tasks;
using ecoAPM.NET.Agent;
using Microsoft.AspNetCore.Http;

namespace ecoAPM.NET.CoreMiddleware
{
    public class ecoAPMMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAgent _agent;
        private readonly Func<ITimer> newTimer;

        public ecoAPMMiddleware(RequestDelegate next, IAgent agent, Func<ITimer> timerFac)
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
                Time = DateTime.UtcNow,
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
