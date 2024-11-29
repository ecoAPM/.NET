using System.Diagnostics;
using NSubstitute;
using Xunit;

namespace ecoAPM.Agent.Tests;

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
	public async Task TimerCanGetTimeFromStopwatch()
	{
		//arrange
		var stopwatch = Substitute.For<Stopwatch>();
		var timer = new Timer(stopwatch);
		timer.Start();
		await Task.Delay(1);
		timer.Stop();

		//act
		var result = timer.CurrentTime;

		//assert
		Assert.Equal(stopwatch.Elapsed.TotalMilliseconds, result);
	}

	[Fact]
	public void TimerCanTimeAction()
	{
		//arrange
		var stopwatch = Substitute.For<Stopwatch>();
		var timer = new Timer(stopwatch);

		//act
		async void Action() => await Task.Delay(1);
		var result = timer.Time(Action);

		//assert
		Assert.Equal(stopwatch.Elapsed.TotalMilliseconds, result);
	}

	[Fact]
	public async Task TimerCanTimeTask()
	{
		//arrange
		var stopwatch = Substitute.For<Stopwatch>();
		var timer = new Timer(stopwatch);

		//act
		var task = Task.Delay(1);
		var result = await timer.Time(task);

		//assert
		Assert.Equal(stopwatch.Elapsed.TotalMilliseconds, result);
	}
}