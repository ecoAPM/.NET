using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ecoAPM.Agent;

/// <summary>Sends batched requests to an ecoAPM server</summary>
public class QueuedAgent : Agent
{
	private readonly List<Request> _requestQueue = new();
	private readonly TimeSpan _sendInterval;

	/// <summary>Returns if the agent is currently running</summary>
	public bool IsRunning { get; private set; }

	/// <summary>Creates a new agent</summary>
	/// <param name="config">The configuration to use to connect to the server</param>
	/// <param name="httpClient">The HTTP client to send requests via</param>
	/// <param name="loggerFactory">The logger for logging detailed information about communications</param>
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
			var toSend = GetOutstandingRequests();
			if (toSend.Any())
				await PostRequests(toSend);
		}
	}

	/// <summary>Retrieves any requests that still need to be sent</summary>
	/// <returns>The outstanding requests</returns>
	public IReadOnlyCollection<Request> GetOutstandingRequests()
		=> _requestQueue.ToArray();

	/// <summary>Adds a request to the queue to be sent</summary>
	/// <param name="request">The request to send</param>
	public override async Task Send(Request request)
		=> await Task.Run(() => _requestQueue.Add(request));

	private bool _sending;

	/// <summary>Sends a batch of requests to the configured ecoAPM server</summary>
	/// <param name="requests">The requests to send</param>
	public async Task PostRequests(IReadOnlyCollection<Request> requests)
	{
		_sending = true;
		try
		{
			_logger?.Log(LogLevel.Debug, "Sending {count} request{s} to {URL}", requests.Count, requests.Count > 1 ? "s" : "", _requestURL);
			var content = GetPostContent(requests);
			var response = await _httpClient.PostAsync(_requestURL, content);
			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException($"Requests were not accepted: {(int)response.StatusCode} {response.StatusCode} {await response.Content.ReadAsStringAsync()}");
			}

			_requestQueue.RemoveAll(requests.Contains);
			_logger?.Log(LogLevel.Information, "Sent {count} request{s} to {URL}", requests.Count, requests.Count > 1 ? "s" : "", _requestURL);
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

	private static HttpContent GetPostContent(IEnumerable<Request> requests)
		=> new StringContent(JsonSerializer.Serialize(requests), Encoding.UTF8, "application/json");

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_logger?.Log(LogLevel.Debug, "Shutting down agent");
			IsRunning = false;
			SpinWait.SpinUntil(() => !_sending);
			var leftover = GetOutstandingRequests();
			var timeout = _sendInterval.TotalMilliseconds > int.MaxValue ? int.MaxValue : (int)_sendInterval.TotalMilliseconds;
			PostRequests(leftover).Wait(timeout);
		}

		base.Dispose(disposing);
	}
}