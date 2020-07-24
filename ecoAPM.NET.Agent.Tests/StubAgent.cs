using System;
using System.Net.Http;

namespace ecoAPM.NET.Agent.Tests
{
    public class StubAgent : Agent
    {
        public Uri AddEventURL => _addEventURL;
        public HttpClient HttpClient => _httpClient;

        public StubAgent(ServerConfig config, HttpClient httpClient) : base(config, httpClient, null)
        {
        }

        public override void Send(Event e) => throw new NotImplementedException();
    }
}