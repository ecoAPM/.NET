using System.Diagnostics;
using System.Threading;
using NSubstitute;
using Xunit;

namespace ecoAPM.NET.Agent.Tests
{
    public class TimerTests
    {
        [Fact]
        public void TimerCanStartStopwatch()
        {
            //arrange
            var stopwatch = Substitute.For<Stopwatch>();
            var timer = new Timer(stopwatch);

            //act
            timer.Start();

            //assert
            stopwatch.Received().Start();
        }

        [Fact]
        public void TimerCanStopStopwatch()
        {
            //arrange
            var stopwatch = Substitute.For<Stopwatch>();
            var timer = new Timer(stopwatch);

            //act
            timer.Stop();

            //assert
            stopwatch.Received().Stop();
        }

        [Fact]
        public void TimerCanGetTimeFromStopwatch()
        {
            //arrange
            var stopwatch = Substitute.For<Stopwatch>();
            var timer = new Timer(stopwatch);
            timer.Start();
            Thread.Sleep(1);
            timer.Stop();

            //act
            var result = timer.CurrentTime;

            //assert
            Assert.InRange(result, 1, 10);
        }

        [Fact]
        public void TimerCanTimeAction()
        {
            //arrange
            var stopwatch = Substitute.For<Stopwatch>();
            var timer = new Timer(stopwatch);

            //act
            var result = timer.Time(() => Thread.Sleep(1));

            //assert
            Assert.InRange(result, 1, 10);
        }
    }
}
