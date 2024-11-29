using System.Diagnostics;

namespace ecoAPM.Agent;

/// <summary>Measures the amount of time an </summary>
public class Timer : ITimer
{
	private readonly Stopwatch _stopwatch;

	/// <summary>Creates a new timer</summary>
	/// <param name="stopwatch">A specific stopwatch to use, otherwise a new one will be created</param>
	public Timer(Stopwatch? stopwatch = null)
		=> _stopwatch = stopwatch ?? new Stopwatch();

	/// <summary>Times a given action</summary>
	/// <param name="action">The action to time</param>
	/// <returns>The number of milliseconds the action took</returns>
	public double Time(Action action)
	{
		Start();
		action.Invoke();
		Stop();
		return CurrentTime;
	}

	/// <summary>Times a given task</summary>
	/// <param name="task">The task to time</param>
	/// <returns>The number of milliseconds the task took</returns>
	public async Task<double> Time(Task task)
	{
		Start();
		await task;
		Stop();
		return CurrentTime;
	}

	/// <summary>Starts the timer</summary>
	public void Start() => _stopwatch.Start();

	/// <summary>Stops the timer</summary>
	public void Stop() => _stopwatch.Stop();

	/// <summary>Retrieves the current timer's elapsed time in milliseconds</summary>
	public double CurrentTime => _stopwatch.Elapsed.TotalMilliseconds;
}