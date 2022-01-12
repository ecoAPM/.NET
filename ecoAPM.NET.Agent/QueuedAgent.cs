using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ecoAPM.NET.Agent;

public class QueuedAgent : Agent
{
	private readonly List<Request> _requestQueue = new();
	private readonly TimeSpan _sendInterval;

	public bool IsRunning { get; private set; }

	public QueuedAgent(IServerConfig config, HttpClient httpClient, ILoggerFactory? loggerFactory = null, TimeSpan? sendInterval = null)
		: base(config, httpClient, loggerFactory)
	{
		_sendInterval = sendInterval ?? TimeSpan.FromSeconds(5);
		IsRunning = true;
		Task.Run(RunSender);
	}

	private async Task RunSender()
	{
		_logger?.Log(LogLevel.Debug, "Starting agent");
		while (IsRunning)
		{
			Thread.Sleep(_sendInterval);
			var toSend = GetRequestsToSend();
			if (toSend.Any())
				await SendRequests(toSend);
		}
	}

	public IList<Request> GetRequestsToSend()
		=> _requestQueue.ToList();

	public async Task SendRequests(ICollection<Request> requests)
	{
		try
		{
			_logger?.Log(LogLevel.Debug, $"Sending {requests.Count} request{(requests.Count > 1 ? "s" : "")} to {_requestURL}");
			var content = GetPostContent(requests);
			await _httpClient.PostAsync(_requestURL, content);
			_requestQueue.RemoveAll(requests.Contains);
			_logger?.Log(LogLevel.Information, $"Sent {requests.Count} request{(requests.Count > 1 ? "s" : "")} to {_requestURL}");
		}
		catch (Exception ex)
		{
			_logger?.Log(LogLevel.Warning, ex, "Failed to send requests");
		}
	}

	public static HttpContent GetPostContent(IEnumerable<Request> requests)
		=> new StringContent(JsonSerializer.Serialize(requests), Encoding.UTF8, "application/json");

	public override async Task Send(Request request)
		=> await Task.Run(() => _requestQueue.Add(request));

	public override void Dispose()
	{
		_logger?.Log(LogLevel.Debug, "Shutting down agent");
		IsRunning = false;
		Thread.Sleep(_sendInterval);
		SendRequests(GetRequestsToSend()).Wait(_sendInterval);
		base.Dispose();
	}
}