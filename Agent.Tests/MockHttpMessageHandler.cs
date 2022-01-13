using System.Net;

namespace ecoAPM.Agent.Tests;

internal class MockHttpMessageHandler : HttpMessageHandler
{
	public HttpContent? Posted { get; private set; }
	private readonly HttpStatusCode _response;

	public MockHttpMessageHandler(HttpStatusCode response = HttpStatusCode.OK)
		=> _response = response;

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (request.Method == HttpMethod.Post)
			Posted = request.Content;

		var msg = new HttpResponseMessage(_response);
		return await Task.FromResult(msg);
	}
}