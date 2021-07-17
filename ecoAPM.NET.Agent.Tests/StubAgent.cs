using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ecoAPM.NET.Agent.Tests
{
    public class StubAgent : Agent
    {
        public Uri AddEventURL => _addEventURL;
        public HttpClient HttpClient => _httpClient;

        public StubAgent(ServerConfig config, HttpClient httpClient) : base(config, httpClient, null)
        {
        }

        public override Task Send(Event e) => throw new NotImplementedException();
    }
}