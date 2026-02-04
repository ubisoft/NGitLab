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
    private static readonly Lazy<HttpClient> s_defaultClient = new(() => CreateHttpClient(null, null));

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
    public static HttpClient CreateHttpClient(IWebProxy proxy, TimeSpan? timeout)
    {
        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

        if (proxy != null)
        {
            handler.Proxy = proxy;
            handler.UseProxy = true;
        }

        var client = new HttpClient(handler);

        if (timeout.HasValue)
        {
            client.Timeout = timeout.Value;
        }

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
        if (options.HttpClientFactory != null)
        {
            return options.HttpClientFactory(options);
        }

        // Use singleton if no custom proxy or timeout
        if (options.Proxy == null && !options.HttpClientTimeout.HasValue)
        {
            return DefaultClient;
        }

        // Create new instance for custom configuration
        return CreateHttpClient(options.Proxy, options.HttpClientTimeout);
    }
}
