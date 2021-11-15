using System;

namespace ecoAPM.NET.Agent;

public interface ITimer
{
	double CurrentTime { get; }

	void Start();
	void Stop();
	double Time(Action action);
}
