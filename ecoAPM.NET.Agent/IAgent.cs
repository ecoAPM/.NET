namespace ecoAPM.NET.Agent;

public interface IAgent : IDisposable
{
	Task Send(Event e);
}