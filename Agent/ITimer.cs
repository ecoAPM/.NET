namespace ecoAPM.Agent;

public interface ITimer
{
	double CurrentTime { get; }

	void Start();
	void Stop();
	double Time(Action action);
	Task<double> Time(Task task);
}