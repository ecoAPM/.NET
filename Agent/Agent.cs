using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ecoAPM.Agent;

/// <summary>
/// Sends single requests to an ecoAPM server
/// </summary>
public class Agent : IAgent, IDisposable
{
	protected readonly Uri _requestURL;
	protected readonly HttpClient _httpClient;
	protected readonly ILogger? _logger;

	/// <summary>Creates a new agent</summary>
	/// <param name="config">The configuration to use to connect to the server</param>
	/// <param name="httpClient">The HTTP client to send requests via</param>
	/// <param name="loggerFactory">The logger for logging detailed information about communications</param>
	public Agent(IServerConfig config, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
	{
		_requestURL = new Uri(config.BaseURL + "requests");
		_httpClient = httpClient;
		var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(config.APIKey.ToString()));
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);
		_logger = loggerFactory?.CreateLogger("ecoAPM");
	}

	public virtual async Task Send(Request request)
	{
		try
		{
			_logger?.Log(LogLevel.Debug, "Sending request to {URL}", _requestURL);
			var response = await _httpClient.PostAsync(_requestURL, GetPostContent(request));
			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException($"Requests were not accepted: {(int)response.StatusCode} {response.StatusCode} {await response.Content.ReadAsStringAsync()}");
			}
		}
		catch (Exception ex)
		{
			_logger?.Log(LogLevel.Warning, ex, "Failed to send request");
		}
	}

	private static HttpContent GetPostContent(Request request)
		=> new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_httpClient.Dispose();
		}
	}
}