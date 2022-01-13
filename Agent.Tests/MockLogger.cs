using System.Text;
using Microsoft.Extensions.Logging;

namespace ecoAPM.Agent.Tests;

public class MockLogger : ILogger
{
	public StringBuilder Output { get; } = new();

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		=> Output.AppendLine(logLevel + ": " + formatter(state, exception));

	public bool IsEnabled(LogLevel logLevel)
		=> true;

	public IDisposable BeginScope<TState>(TState state)
		=> throw new NotImplementedException();
}