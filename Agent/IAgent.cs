namespace ecoAPM.Agent;

public interface IAgent : IDisposable
{
	Task Send(Request request);
}