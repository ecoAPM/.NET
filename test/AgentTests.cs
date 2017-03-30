using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreAPM.Events.Model;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace CoreAPM.DotNet.Agent.Tests
{
    public class AgentTests
    {
        [Fact]
        public void AddEventUrlCombinedCorrectlyWithSlashInBase()
        {
            //arrange
            var config = new Config(new Uri("http://localhost/"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();

            //act
            var agent = new StubAgent(config, httpClient);

            //assert
            Assert.Equal("http://localhost/add", agent.AddEventURL.AbsoluteUri);
        }

        [Fact]
        public void AddEventUrlCombinedCorrectlyWithoutSlashInBase()
        {
            //arrange
            var config = new Config(new Uri("http://localhost"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();

            //act
            var agent = new StubAgent(config, httpClient);

            //assert
            Assert.Equal("http://localhost/add", agent.AddEventURL.AbsoluteUri);
        }

        [Fact]
        public void APIKeyAddedToAuthHeader()
        {
            //arrange
            var apiKey = Guid.NewGuid();
            var config = new Config(new Uri("http://localhost/"), apiKey);
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
            var config = new Config(new Uri("http://localhost/"), Guid.NewGuid());
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
            var config = new Config(new Uri("http://localhost/"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();
            var agent = new Agent(config, httpClient);

            //act
            agent.Send(new Event());

            //assert
            await httpClient.ReceivedWithAnyArgs().PostAsync(Arg.Any<Uri>(), Arg.Any<HttpContent>());
        }

        [Fact]
        public async Task EventCanConvertToJSON()
        {
            //arrange
            var e = new Event {Action = "a1"};

            //act
            var json = Agent.GetJSON(e);

            //assert
            Assert.Equal("a1", JObject.Parse(json)["Action"]);
        }
    }
}