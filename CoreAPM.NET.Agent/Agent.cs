﻿using System;
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

        public Agent(IServerConfig config, HttpClient httpClient, ILoggerFactory loggerFactory = null)
        {
            _addEventURL = new Uri(config.BaseURL + "events");
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.APIKey.ToString().ToLower());
            _logger = loggerFactory?.CreateLogger("CoreAPM");
        }

        public static HttpContent GetPostContent(Event e) => new StringContent(JObject.FromObject(e).ToString());

        public virtual void Send(Event e)
        {
            try
            {
                _logger?.Log(LogLevel.Debug, $"Sending event to {_addEventURL}");
                _httpClient.PostAsync(_addEventURL, GetPostContent(e));
            }
            catch (Exception ex)
            {
                _logger?.Log(LogLevel.Warning, ex, "Failed to send event");
            }
        }

        public virtual void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}