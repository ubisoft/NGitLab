using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NGitLab.Http;

internal sealed class CustomHttpMessageHandler : DelegatingHandler
{
    private readonly IHttpMessageHook _hook;

    public CustomHttpMessageHandler(HttpMessageHandler innerHandler, IHttpMessageHook hook)
    {
        InnerHandler = innerHandler;
        _hook = hook;
    }

#if NET8_0_OR_GREATER
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _hook.BeforeSending(request);
        var response = base.Send(request, cancellationToken);
        _hook.AfterReceived(response);
        return response;
    }
#endif

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _hook.BeforeSending(request);
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        _hook.AfterReceived(response);
        return response;
    }
}
