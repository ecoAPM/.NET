using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CoreAPM.NET.Agent
{
    public class Agent : IAgent
    {
        protected readonly Uri _addEventURL;
        protected readonly HttpClient _httpClient;
        protected readonly ILogger _logger;

        public Agent(IServerConfig config, HttpClient httpClient, ILogger logger = null)
        {
            _addEventURL = new Uri(config.BaseURL + "events");
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.APIKey.ToString().ToLower());
            _logger = logger;
        }

        public static HttpContent GetPostContent(Event e) => new StringContent(JObject.FromObject(e).ToString());

        public virtual void Send(Event e)
        {
            try
            {
                _logger?.LogDebug($"Sending event to {_addEventURL}");
                _httpClient.PostAsync(_addEventURL, GetPostContent(e));
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex.ToString());
            }
        }

        public virtual void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}