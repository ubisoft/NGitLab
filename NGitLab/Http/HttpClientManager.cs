using System;
using System.Net;
using System.Net.Http;

namespace NGitLab.Http;

/// <summary>
/// Manages the lifecycle of HttpClient instances.
/// Uses a singleton HttpClient for default scenarios to improve connection pooling.
/// Creates new instances only when custom configuration (proxy, timeout) is required.
/// </summary>
internal static class HttpClientManager
{
    /// <summary>
    /// Configure the default client-side timeout when calling GitLab.
    /// Some GitLab endpoints are really slow, so use a much larger value
    /// than .NET's 100-second default.
    /// </summary>
    private static readonly TimeSpan s_defaultHttpClientTimeout = TimeSpan.FromMinutes(5);

    private static readonly Lazy<HttpClient> s_defaultClient = new(() => CreateHttpClient(proxy: null, timeout: null, msgHook: null));

    /// <summary>
    /// Gets the singleton HttpClient instance for default scenarios.
    /// </summary>
    public static HttpClient DefaultClient => s_defaultClient.Value;

    /// <summary>
    /// Creates a new HttpClient with the specified configuration.
    /// </summary>
    /// <param name="proxy">Optional proxy configuration.</param>
    /// <param name="timeout">Optional timeout value.</param>
    /// <returns>A configured HttpClient instance.</returns>
    private static HttpClient CreateHttpClient(IWebProxy proxy, TimeSpan? timeout, IHttpMessageHook msgHook)
    {
        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

        if (proxy is not null)
        {
            handler.Proxy = proxy;
            handler.UseProxy = true;
        }

        HttpMessageHandler handlerChain;
        if (msgHook is null)
        {
            handlerChain = handler;
        }
        else
        {
            handlerChain = new CustomHttpMessageHandler(handler, msgHook);
        }

        var client = new HttpClient(handlerChain)
        {
            Timeout = timeout ?? s_defaultHttpClientTimeout,
        };

        return client;
    }

    /// <summary>
    /// Gets or creates an HttpClient based on the provided options.
    /// Returns the singleton instance if no custom configuration is needed.
    /// </summary>
    /// <param name="options">The request options containing proxy and timeout configuration.</param>
    /// <returns>An HttpClient instance.</returns>
    public static HttpClient GetOrCreateHttpClient(RequestOptions options)
    {
        // Use singleton if no custom proxy or timeout
        if (options.Proxy is null && options.HttpClientTimeout is null && options.MessageHook is null)
        {
            return DefaultClient;
        }

        // Create new instance for custom configuration
        return CreateHttpClient(options.Proxy, options.HttpClientTimeout, options.MessageHook);
    }
}
