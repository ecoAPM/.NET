using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ecoAPM.NET.Agent;

public class QueuedAgent : Agent
{
	private readonly List<Request> _requestQueue = new();
	private readonly TimeSpan _sendInterval;

	public bool IsRunning { get; private set; }

	public QueuedAgent(IServerConfig config, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
		: base(config, httpClient, loggerFactory)
	{
		_sendInterval = config.Interval;
		IsRunning = true;
		Task.Run(RunSender);
	}

	private async Task RunSender()
	{
		_logger?.Log(LogLevel.Debug, "Starting agent");
		while (IsRunning)
		{
			await Task.Delay(_sendInterval);
			var toSend = GetRequestsToSend();
			if (toSend.Any())
				await SendRequests(toSend);
		}
	}

	public IList<Request> GetRequestsToSend()
		=> _requestQueue.ToList();

	private bool _sending;

	public async Task SendRequests(ICollection<Request> requests)
	{
		_sending = true;
		try
		{
			_logger?.Log(LogLevel.Debug, $"Sending {requests.Count} request{(requests.Count > 1 ? "s" : "")} to {_requestURL}");
			var content = GetPostContent(requests);
			var response = await _httpClient.PostAsync(_requestURL, content);
			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException($"Requests were not accepted: {(int)response.StatusCode} {response.StatusCode} {await response.Content.ReadAsStringAsync()}");
			}

			_requestQueue.RemoveAll(requests.Contains);
			_logger?.Log(LogLevel.Information, $"Sent {requests.Count} request{(requests.Count > 1 ? "s" : "")} to {_requestURL}");
		}
		catch (Exception ex)
		{
			_logger?.Log(LogLevel.Warning, ex, "Failed to send requests");
		}
		finally
		{
			_sending = false;
		}
	}

	public static HttpContent GetPostContent(IEnumerable<Request> requests)
		=> new StringContent(JsonSerializer.Serialize(requests), Encoding.UTF8, "application/json");

	public override async Task Send(Request request)
		=> await Task.Run(() => _requestQueue.Add(request));

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_logger?.Log(LogLevel.Debug, "Shutting down agent");
			IsRunning = false;
			SpinWait.SpinUntil(() => !_sending);
			var leftover = GetRequestsToSend();
			var timeout = _sendInterval.TotalMilliseconds > int.MaxValue ? int.MaxValue : (int)_sendInterval.TotalMilliseconds;
			SendRequests(leftover).Wait(timeout);
		}

		base.Dispose(disposing);
	}
}