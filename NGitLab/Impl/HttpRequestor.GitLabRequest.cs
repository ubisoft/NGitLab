using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Http;
using NGitLab.Impl.Json;
using NGitLab.Models;

namespace NGitLab.Impl;

public partial class HttpRequestor
{
    /// <summary>
    /// A single request to GitLab, that can be retried.
    /// </summary>
    private sealed class GitLabRequest
    {
        public Uri Url { get; }

        private object Data { get; }

        public string JsonData { get; }

        public FormDataContent FormData { get; }

        public UrlEncodedContent UrlEncodedData { get; }

        private MethodType Method { get; }

        private readonly Dictionary<string, string> _headers = new(StringComparer.OrdinalIgnoreCase);

        private readonly HttpClient _httpClient;
        private readonly RequestOptions _options;

        private bool HasOutput
            => (Method == MethodType.Delete || Method == MethodType.Post || Method == MethodType.Put || Method == MethodType.Patch)
                && Data != null;

        public GitLabRequest(Uri url, MethodType method, object data, string apiToken, RequestOptions options, HttpClient httpClient)
        {
            Method = method;
            Url = url;
            Data = data;
            _httpClient = httpClient;
            _options = options ?? RequestOptions.Default;

            // Add headers
            _headers.Add("Accept-Encoding", "gzip");

            if (!string.IsNullOrEmpty(options?.Sudo))
            {
                _headers.Add("Sudo", options.Sudo);
            }

            if (apiToken != null)
            {
                // Use the 'Authorization: Bearer token' header as this provides flexibility to use
                // personal, project, group and OAuth tokens. The 'PRIVATE-TOKEN' header does not
                // provide OAuth token support.
                // Reference: https://docs.gitlab.com/ee/api/rest/#personalprojectgroup-access-tokens
                var authValue = string.Concat("Bearer ", apiToken);
                _headers.Add("Authorization", authValue);
            }

            if (!string.IsNullOrEmpty(options?.UserAgent))
            {
                _headers.Add("User-Agent", options.UserAgent);
            }

            if (data is FormDataContent formData)
            {
                FormData = formData;
            }
            else if (Data is UrlEncodedContent urlEncodedData)
            {
                UrlEncodedData = urlEncodedData;
            }
            else if (data != null)
            {
                JsonData = Serializer.Serialize(data);
            }
        }

        public WebResponse GetResponse(RequestOptions options)
        {
            Func<WebResponse> getResponseImpl = () => GetResponseImpl(options);

            return getResponseImpl.Retry(options.ShouldRetry,
                options.RetryInterval,
                options.RetryCount,
                options.IsIncremental);
        }

        public Task<WebResponse> GetResponseAsync(RequestOptions options, CancellationToken cancellationToken)
        {
            Func<Task<WebResponse>> getResponseImpl = () => GetResponseImplAsync(options, cancellationToken);

            return getResponseImpl.RetryAsync(options.ShouldRetry,
                options.RetryInterval,
                options.RetryCount,
                options.IsIncremental);
        }

        private WebResponse GetResponseImpl(RequestOptions options)
        {
            try
            {
                var request = CreateHttpRequestMessage();
#if NET8_0_OR_GREATER
                var response = _httpClient.Send(request);
#else
                var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
#endif
                if (!response.IsSuccessStatusCode)
                {
                    HandleHttpResponseError(response);
                }

                return new HttpResponseMessageWrapper(response);
            }
            catch (HttpRequestException ex)
            {
                HandleHttpRequestException(ex);
                throw;
            }
        }

        private async Task<WebResponse> GetResponseImplAsync(RequestOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var request = CreateHttpRequestMessage();
                var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    await HandleHttpResponseErrorAsync(response).ConfigureAwait(false);
                }

                return new HttpResponseMessageWrapper(response);
            }
            catch (HttpRequestException ex)
            {
                HandleHttpRequestException(ex);
                throw;
            }
        }

        private HttpRequestMessage CreateHttpRequestMessage()
        {
            var request = new HttpRequestMessage(GetHttpMethod(), Url);

            // Add headers
            foreach (var header in _headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // Add content
            if (HasOutput)
            {
                if (FormData != null)
                {
                    request.Content = CreateFileContent();
                }
                else if (UrlEncodedData != null)
                {
                    request.Content = new FormUrlEncodedContent(UrlEncodedData.Values);
                }
                else if (JsonData != null)
                {
                    request.Content = new StringContent(JsonData, Encoding.UTF8, "application/json");
                }
            }
            else if (Method == MethodType.Put)
            {
                request.Content = new StringContent(string.Empty);
            }

            return request;
        }

        private HttpMethod GetHttpMethod()
        {
            return Method switch
            {
                MethodType.Get => HttpMethod.Get,
                MethodType.Post => HttpMethod.Post,
                MethodType.Put => HttpMethod.Put,
                MethodType.Delete => HttpMethod.Delete,
                MethodType.Head => HttpMethod.Head,
                MethodType.Options => HttpMethod.Options,
#if NET8_0_OR_GREATER
                MethodType.Patch => HttpMethod.Patch,
#else
                MethodType.Patch => new HttpMethod("PATCH"),
#endif
                _ => throw new NotSupportedException($"HTTP method {Method} is not supported."),
            };
        }

        private MultipartFormDataContent CreateFileContent()
        {
            if (Data is not FormDataContent formData)
                return null;

            var boundary = $"--------------------------{DateTime.UtcNow.Ticks.ToStringInvariant()}";
            var content = new MultipartFormDataContent(boundary);
            content.Add(new StreamContent(formData.Stream), "file", formData.Name);
            return content;
        }

        private void HandleHttpResponseError(HttpResponseMessage response)
        {
            string jsonString;
            using (var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
            using (var reader = new StreamReader(stream))
            {
                jsonString = reader.ReadToEnd();
            }

            var errorMessage = ExtractErrorMessage(jsonString, out var errorDetails);
            var exceptionMessage =
                $"GitLab server returned an error ({response.StatusCode}): {errorMessage}. " +
                $"Original call: {Method} {Url}";

            if (JsonData != null)
            {
                exceptionMessage += $". With data {JsonData}";
            }

            response.Dispose();

            throw new GitLabException(exceptionMessage)
            {
                OriginalCall = Url,
                ErrorObject = errorDetails,
                StatusCode = response.StatusCode,
                ErrorMessage = errorMessage,
                MethodType = Method,
            };
        }

        private async Task HandleHttpResponseErrorAsync(HttpResponseMessage response)
        {
            string jsonString;
            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var reader = new StreamReader(stream))
            {
                jsonString = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            var errorMessage = ExtractErrorMessage(jsonString, out var errorDetails);
            var exceptionMessage =
                $"GitLab server returned an error ({response.StatusCode}): {errorMessage}. " +
                $"Original call: {Method} {Url}";

            if (JsonData != null)
            {
                exceptionMessage += $". With data {JsonData}";
            }

            response.Dispose();

            throw new GitLabException(exceptionMessage)
            {
                OriginalCall = Url,
                ErrorObject = errorDetails,
                StatusCode = response.StatusCode,
                ErrorMessage = errorMessage,
                MethodType = Method,
            };
        }

        private void HandleHttpRequestException(HttpRequestException ex)
        {
            // Handle connection-level errors (no response received)
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

#if NET8_0_OR_GREATER
            // HttpRequestException in .NET 5+ includes StatusCode
            if (ex.StatusCode.HasValue)
            {
                statusCode = ex.StatusCode.Value;
            }
#endif

            var exceptionMessage =
                $"GitLab server returned an error ({statusCode}): {ex.Message}. " +
                $"Original call: {Method} {Url}";

            if (JsonData != null)
            {
                exceptionMessage += $". With data {JsonData}";
            }

            throw new GitLabException(exceptionMessage, ex)
            {
                OriginalCall = Url,
                ErrorObject = null,
                StatusCode = statusCode,
                ErrorMessage = ex.Message,
                MethodType = Method,
            };
        }

        /// <summary>
        /// Parse the error that GitLab returns. GitLab returns structured errors but has a lot of them
        /// Here we try to be generic.
        /// </summary>
        /// <param name="json">JSON description of the error</param>
        /// <param name="errorDetails">Dictionary of JSON properties</param>
        /// <returns>Parsed error message</returns>
        private static string ExtractErrorMessage(string json, out IDictionary<string, object> errorDetails)
        {
            errorDetails = null;
            if (string.IsNullOrEmpty(json))
                return "Empty Response";

            if (!Serializer.TryDeserializeObject(json, out var errorObject))
                return $"Response cannot be deserialized ({json})";

            string message = null;
            var details = new Dictionary<string, object>(StringComparer.Ordinal);
            if (errorObject is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
            {
                var objectEnumerator = jsonElement.EnumerateObject();
                foreach (var property in objectEnumerator)
                {
                    details[property.Name] = property.Value;
                }

                if (!details.TryGetValue("message", out var messageValue))
                {
                    details.TryGetValue("error", out messageValue);
                }

                errorDetails = details;
                message = messageValue?.ToString();
            }

            return message ?? $"Error message cannot be parsed ({json})";
        }

        /// <summary>
        /// Wrapper to make HttpResponseMessage compatible with WebResponse API
        /// </summary>
        private sealed class HttpResponseMessageWrapper : WebResponse
        {
            private readonly HttpResponseMessage _response;

            public HttpResponseMessageWrapper(HttpResponseMessage response)
            {
                _response = response ?? throw new ArgumentNullException(nameof(response));
            }

            public override WebHeaderCollection Headers
            {
                get
                {
                    var headers = new WebHeaderCollection();
                    foreach (var header in _response.Headers)
                    {
                        foreach (var value in header.Value)
                        {
                            headers.Add(header.Key, value);
                        }
                    }

                    if (_response.Content?.Headers != null)
                    {
                        foreach (var header in _response.Content.Headers)
                        {
                            foreach (var value in header.Value)
                            {
                                headers.Add(header.Key, value);
                            }
                        }
                    }

                    return headers;
                }
            }

            public override Stream GetResponseStream()
            {
                // ReadAsStreamAsync returns a stream that can be read synchronously
                return _response.Content?.ReadAsStreamAsync().GetAwaiter().GetResult();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _response?.Dispose();
                }

                base.Dispose(disposing);
            }
        }
    }
}
