using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ecoAPM.NET.Agent
{
    public class QueuedAgent : Agent
    {
        private readonly List<Event> _eventQueue = new List<Event>();
        private readonly TimeSpan _sendInterval;

        public bool IsRunning { get; private set; }

        public QueuedAgent(IServerConfig config, HttpClient httpClient, ILoggerFactory loggerFactory = null, TimeSpan? sendInterval = null)
            : base(config, httpClient, loggerFactory)
        {
            _sendInterval = sendInterval ?? TimeSpan.FromSeconds(1);
            IsRunning = true;
            Task.Run(RunSender);
        }

        private async Task RunSender()
        {
            _logger?.Log(LogLevel.Debug, "Starting agent");
            while (IsRunning)
            {
                Thread.Sleep(_sendInterval);
                var eventsToSend = GetEventsToSend();
                if (eventsToSend.Any())
                    await SendEvents(eventsToSend);
            }
        }

        public IList<Event> GetEventsToSend() => _eventQueue.ToList();

        public async Task SendEvents(ICollection<Event> eventsToSend)
        {
            try
            {
                _logger?.Log(LogLevel.Debug, $"Sending {eventsToSend.Count} event{(eventsToSend.Count > 1 ? "s" : "")} to {_addEventURL}");
                var content = GetPostContent(eventsToSend);
                await _httpClient.PostAsync(_addEventURL, content);
                _eventQueue.RemoveAll(eventsToSend.Contains);
                _logger?.Log(LogLevel.Information, $"Sent {eventsToSend.Count} event{(eventsToSend.Count > 1 ? "s" : "")} to {_addEventURL}");
            }
            catch (Exception ex)
            {
                _logger?.Log(LogLevel.Warning, ex, "Failed to send events");
            }
        }

        public static HttpContent GetPostContent(IEnumerable<Event> eventsToSend) => new StringContent(JArray.FromObject(eventsToSend).ToString(), Encoding.UTF8, "application/json");

        public override async Task Send(Event e) => await Task.Run(() => _eventQueue.Add(e));

        public override void Dispose()
        {
            _logger?.Log(LogLevel.Debug, "Shutting down agent");
            IsRunning = false;
            Thread.Sleep(_sendInterval);
            SendEvents(GetEventsToSend()).Wait(_sendInterval);
            base.Dispose();
        }
    }
}
