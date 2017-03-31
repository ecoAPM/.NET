using System;
using System.Net.Http;
using System.Net.Http.Headers;
using CoreAPM.Events.Model;
using Newtonsoft.Json.Linq;

namespace CoreAPM.DotNet.Agent
{
    public class Agent : IAgent
    {
        protected readonly HttpClient _httpClient;
        protected readonly Uri _addEventURL;

        public Agent(IConfig config, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.APIKey.ToString().ToLower());
            _addEventURL = new Uri(config.EventsAPI + (config.EventsAPI.ToString().EndsWith("/") ? "add" : "/add"));
        }

        public static HttpContent GetPostContent(Event e) => new StringContent(JObject.FromObject(e).ToString());

        public virtual void Send(Event e) => _httpClient.PostAsync(_addEventURL, GetPostContent(e));

        public virtual void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}