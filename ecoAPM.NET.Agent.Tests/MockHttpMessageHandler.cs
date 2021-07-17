using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ecoAPM.NET.Agent.Tests
{
    internal class MockHttpMessageHandler : HttpMessageHandler
    {
        public bool Posted { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post)
                Posted = true;

            return await Task.FromResult(new HttpResponseMessage());
        }
    }
}