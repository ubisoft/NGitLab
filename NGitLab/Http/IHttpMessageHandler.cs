using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NGitLab.Http;

/// <summary>
/// Provides an extensibility point for customizing HTTP request behavior.
/// This interface replaces the obsolete virtual methods on <see cref="RequestOptions"/>.
/// </summary>
public interface IHttpMessageHandler
{
    /// <summary>
    /// Sends an HTTP request synchronously.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <returns>The HTTP response message.</returns>
    HttpResponseMessage Send(HttpRequestMessage request);

    /// <summary>
    /// Sends an HTTP request asynchronously.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}
