using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace ecoAPM.NET.Agent.Tests
{
    public class AgentTests
    {
        [Fact]
        public void AddEventUrlCombinedCorrectlyWithSlashInBase()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();

            //act
            var agent = new StubAgent(config, httpClient);

            //assert
            Assert.Equal("http://localhost/events", agent.AddEventURL.AbsoluteUri);
        }

        [Fact]
        public void AddEventUrlCombinedCorrectlyWithoutSlashInBase()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();

            //act
            var agent = new StubAgent(config, httpClient);

            //assert
            Assert.Equal("http://localhost/events", agent.AddEventURL.AbsoluteUri);
        }

        [Fact]
        public void APIKeyAddedToAuthHeader()
        {
            //arrange
            var apiKey = Guid.NewGuid();
            var config = new ServerConfig(new Uri("http://localhost/"), apiKey);
            var httpClient = Substitute.For<HttpClient>();

            //act
            var agent = new StubAgent(config, httpClient);

            //assert
            Assert.Contains(apiKey.ToString(), agent.HttpClient.DefaultRequestHeaders.Authorization.ToString());
        }

        [Fact]
        public void HttpClientIsDisposedOnDispose()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();
            var agent = new StubAgent(config, httpClient);

            //act
            agent.Dispose();

            //assert
            httpClient.ReceivedWithAnyArgs().Dispose();
        }

        [Fact]
        public async Task SendPerformsHttpPost()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost/"), Guid.NewGuid());
            var http = new MockHttpMessageHandler();
            var agent = new Agent(config, new HttpClient(http), null);

            //act
            await agent.Send(new Event());

            //assert
            Assert.True(http.Posted);
        }

        [Fact]
        public async Task EventCanConvertToJSON()
        {
            //arrange
            var e = new Event { Action = "a1" };

            //act
            var json = await Agent.GetPostContent(e).ReadAsStringAsync();

            //assert
            Assert.Equal("a1", JObject.Parse(json)["Action"]);
        }
    }
}