using System;
using System.Net.Http;
using CoreAPM.Events.Model;

namespace CoreAPM.DotNet.Agent.Tests
{
    public class StubAgent : Agent
    {
        public Uri AddEventURL => _addEventURL;
        public HttpClient HttpClient => _httpClient;

        public StubAgent(IConfig config, HttpClient httpClient) : base(config, httpClient)
        {
        }

        public override void Send(Event e) => throw new NotImplementedException();
    }
}