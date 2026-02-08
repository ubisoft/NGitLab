using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Http;
using NUnit.Framework;

namespace NGitLab.Tests.Docker;

/// <summary>
/// Store and logs requests
/// </summary>
internal sealed class GitLabTestContextRequestOptions : RequestOptions
{
    private readonly List<HttpRequestMessage> _allRequests = new();
    private static readonly SemaphoreSlim s_semaphoreSlim = new(1, 1);

    private readonly ConcurrentDictionary<HttpRequestMessage, byte[]> _requestContents = new();

    public IReadOnlyList<HttpRequestMessage> AllRequests => _allRequests;

    public GitLabTestContextRequestOptions()
        : base(retryCount: 0, retryInterval: TimeSpan.FromSeconds(1), isIncremental: true)
    {
        UserAgent = "NGitLab.Tests/1.0.0";
        MessageHandler = new TestHttpMessageHandler(this);
    }

    private sealed class TestHttpMessageHandler : IHttpMessageHandler
    {
        private readonly GitLabTestContextRequestOptions _options;
        private readonly HttpClient _httpClient;

        public TestHttpMessageHandler(GitLabTestContextRequestOptions options)
        {
            _options = options;
            _httpClient = HttpClientManager.GetOrCreateHttpClient(options);
        }

        public HttpResponseMessage Send(HttpRequestMessage request)
        {
            lock (_options._allRequests)
            {
                _options._allRequests.Add(request);
            }

            HttpResponseMessage response = null;

            // GitLab is unstable, so let's make sure we don't overload it with many concurrent requests
            s_semaphoreSlim.Wait();
            try
            {
                try
                {
                    response = _httpClient.Send(request);
                }
                catch (HttpRequestException)
                {
                    // Log the request even if it fails
                    _options.LogRequest(request, response);
                    throw;
                }

                _options.LogRequest(request, response);
            }
            finally
            {
                s_semaphoreSlim.Release();
            }

            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            lock (_options._allRequests)
            {
                _options._allRequests.Add(request);
            }

            HttpResponseMessage response = null;

            // GitLab is unstable, so let's make sure we don't overload it with many concurrent requests
            await s_semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                try
                {
                    response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                }
                catch (HttpRequestException)
                {
                    // Log the request even if it fails
                    await _options.LogRequestAsync(request, response).ConfigureAwait(false);
                    throw;
                }

                await _options.LogRequestAsync(request, response).ConfigureAwait(false);
            }
            finally
            {
                s_semaphoreSlim.Release();
            }

            return response;
        }
    }

    private void LogRequest(HttpRequestMessage request, HttpResponseMessage response)
    {
        var sb = new StringBuilder();
        sb.Append(request.Method);
        sb.Append(' ');
        sb.Append(request.RequestUri);
        sb.AppendLine();
        LogHeaders(sb, request.Headers);

        if (request.Content != null)
        {
            byte[] requestContent;
            if (_requestContents.TryGetValue(request, out requestContent) || TryReadRequestContent(request, out requestContent))
            {
                sb.AppendLine();

                if (string.Equals(request.Content.Headers.ContentType?.MediaType, "application/json", StringComparison.Ordinal))
                {
                    sb.AppendLine(Encoding.UTF8.GetString(requestContent));
                }
                else
                {
                    sb.Append("Binary data: ").Append(requestContent.Length).AppendLine(" bytes");
                }

                sb.AppendLine();
            }
        }

        if (response != null)
        {
            sb.AppendLine("----------");

            if (response.RequestMessage?.RequestUri != request.RequestUri)
            {
                sb.Append(request.RequestUri).AppendLine();
            }

            sb.Append((int)response.StatusCode).Append(' ').AppendLine(response.StatusCode.ToString());
            LogHeaders(sb, response.Headers);

            if (string.Equals(response.Content?.Headers.ContentType?.MediaType, "application/json", StringComparison.Ordinal))
            {
                sb.AppendLine();
                try
                {
                    var responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    sb.AppendLine(responseText);
                }
                catch
                {
                    sb.AppendLine("(unable to read response content)");
                }
            }
            else if (response.Content != null)
            {
                var contentLength = response.Content.Headers.ContentLength ?? -1;
                sb.Append("Binary data: ").Append(contentLength).AppendLine(" bytes");
            }
        }

        var logs = sb.ToString();
        TestContext.Out.WriteLine(new string('-', 100) + "\nGitLab request: " + logs);
    }

    private async Task LogRequestAsync(HttpRequestMessage request, HttpResponseMessage response)
    {
        var sb = new StringBuilder();
        sb.Append(request.Method);
        sb.Append(' ');
        sb.Append(request.RequestUri);
        sb.AppendLine();
        LogHeaders(sb, request.Headers);

        if (request.Content != null)
        {
            byte[] requestContent;
            if (_requestContents.TryGetValue(request, out requestContent) || TryReadRequestContent(request, out requestContent))
            {
                sb.AppendLine();

                if (string.Equals(request.Content.Headers.ContentType?.MediaType, "application/json", StringComparison.Ordinal))
                {
                    sb.AppendLine(Encoding.UTF8.GetString(requestContent));
                }
                else
                {
                    sb.Append("Binary data: ").Append(requestContent.Length).AppendLine(" bytes");
                }

                sb.AppendLine();
            }
        }

        if (response != null)
        {
            sb.AppendLine("----------");

            if (response.RequestMessage?.RequestUri != request.RequestUri)
            {
                sb.Append(request.RequestUri).AppendLine();
            }

            sb.Append((int)response.StatusCode).Append(' ').AppendLine(response.StatusCode.ToString());
            LogHeaders(sb, response.Headers);

            if (string.Equals(response.Content?.Headers.ContentType?.MediaType, "application/json", StringComparison.Ordinal))
            {
                sb.AppendLine();
                try
                {
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    sb.AppendLine(responseText);
                }
                catch
                {
                    sb.AppendLine("(unable to read response content)");
                }
            }
            else if (response.Content != null)
            {
                var contentLength = response.Content.Headers.ContentLength ?? -1;
                sb.Append("Binary data: ").Append(contentLength).AppendLine(" bytes");
            }
        }

        var logs = sb.ToString();
        TestContext.Out.WriteLine(new string('-', 100) + "\nGitLab request: " + logs);
    }

    private bool TryReadRequestContent(HttpRequestMessage request, out byte[] content)
    {
        try
        {
            if (request.Content != null)
            {
                // Try to read the content - this may fail if the stream has already been consumed
                content = request.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                return true;
            }
        }
        catch
        {
            // Content stream may have been consumed already
        }

        content = null;
        return false;
    }

    private static void LogHeaders(StringBuilder sb, System.Net.Http.Headers.HttpHeaders headers)
    {
        foreach (var header in headers)
        {
            foreach (var value in header.Value)
            {
                sb.Append(header.Key).Append(": ");
                if (string.Equals(header.Key, "Private-Token", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine("******");
                }
                else if (string.Equals(header.Key, "Authorization", StringComparison.OrdinalIgnoreCase))
                {
                    const string BearerTokenPrefix = "Bearer ";
                    if (value.StartsWith(BearerTokenPrefix, StringComparison.Ordinal))
                        sb.Append(BearerTokenPrefix);
                    sb.AppendLine("******");
                }
                else
                {
                    sb.AppendLine(value);
                }
            }
        }
    }
}
