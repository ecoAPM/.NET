using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ecoAPM.NET.Agent;

public class Agent : IAgent
{
	protected readonly Uri _requestURL;
	protected readonly HttpClient _httpClient;
	protected readonly ILogger? _logger;

	public Agent(IServerConfig config, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
	{
		_requestURL = new Uri(config.BaseURL + "requests");
		_httpClient = httpClient;
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", config.APIKey.ToString().ToLower());
		_logger = loggerFactory?.CreateLogger("ecoAPM");
	}

	public static HttpContent GetPostContent(Request request)
		=> new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

	public virtual async Task Send(Request request)
	{
		try
		{
			_logger?.Log(LogLevel.Debug, $"Sending request to {_requestURL}");
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

	public virtual void Dispose()
	{
		_httpClient.Dispose();
	}
}