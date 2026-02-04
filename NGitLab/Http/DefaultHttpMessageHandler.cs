using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NGitLab.Http;

/// <summary>
/// Default implementation of <see cref="IHttpMessageHandler"/> that wraps HttpClient.
/// </summary>
internal sealed class DefaultHttpMessageHandler : IHttpMessageHandler
{
    private readonly HttpClient _httpClient;

    public DefaultHttpMessageHandler(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public HttpResponseMessage Send(HttpRequestMessage request)
    {
#if NET8_0_OR_GREATER
        return _httpClient.Send(request);
#else
        return _httpClient.SendAsync(request).GetAwaiter().GetResult();
#endif
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        return _httpClient.SendAsync(request, cancellationToken);
    }
}
