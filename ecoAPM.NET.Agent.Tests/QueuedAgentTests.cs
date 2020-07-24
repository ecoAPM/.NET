using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace ecoAPM.NET.Agent.Tests
{
    public class QueuedAgentTests
    {
        [Fact]
        public void SendAddsEventToQueue()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();
            var agent = new QueuedAgent(config, httpClient, null, TimeSpan.Zero);

            //act
            var e = new Event();
            agent.Send(e);

            //assert
            Assert.Contains(e, agent.GetEventsToSend());
        }

        [Fact]
        public void EventsQueueIsClearedOnDispose()
        {
            var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();
            var agent = new QueuedAgent(config, httpClient, null, TimeSpan.Zero);
            agent.Send(new Event());

            //act
            agent.Dispose();

            //assert
            Assert.Empty(agent.GetEventsToSend());
        }

        [Fact]
        public async Task PostContentContainsAllEvents()
        {
            //arrange
            var events = new[] { new Event { Action = "a1" }, new Event { Action = "a2" } };

            //act
            var postContent = QueuedAgent.GetPostContent(events);

            //assert
            var actions = JArray.Parse(await postContent.ReadAsStringAsync()).Select(t => t["Action"]).ToList();
            Assert.Contains("a1", actions);
            Assert.Contains("a2", actions);
        }

        [Fact]
        public async Task SendEventsCallsHttpPost()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();
            var agent = new QueuedAgent(config, httpClient, null, TimeSpan.Zero);
            var events = new[] { new Event { Action = "a1" }, new Event { Action = "a2" } };

            //act
            await agent.SendEvents(events);

            //assert
            await httpClient.ReceivedWithAnyArgs().PostAsync(Arg.Any<Uri>(), Arg.Any<HttpContent>());
        }

        [Fact]
        public async Task SendEventsRemovesSentEventsFromQueue()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();
            var agent = new QueuedAgent(config, httpClient, null);
            var e1 = new Event { Action = "a1" };
            var e2 = new Event { Action = "a2" };
            var e3 = new Event { Action = "a3" };
            agent.Send(e1);
            agent.Send(e2);
            agent.Send(e3);

            //act
            var eventsToSend = new[] { e1, e2 };
            await agent.SendEvents(eventsToSend);

            //assert
            var eventsLeft = agent.GetEventsToSend();
            Assert.DoesNotContain(e1, eventsLeft);
            Assert.DoesNotContain(e2, eventsLeft);
            Assert.Contains(e3, eventsLeft);
        }

        [Fact]
        public void SenderStartedOnConstruction()
        {
            //arrange
            var config = new ServerConfig(new Uri("http://localhost"), Guid.NewGuid());
            var httpClient = Substitute.For<HttpClient>();

            //act
            var agent = new QueuedAgent(config, httpClient, null, TimeSpan.Zero);

            //assert
            Assert.True(agent.IsRunning);
        }
    }
}