using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
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

        public WebHeaderCollection Headers { get; } = [];

        private bool HasOutput
            => (Method == MethodType.Delete || Method == MethodType.Post || Method == MethodType.Put || Method == MethodType.Patch)
                && Data != null;

        public GitLabRequest(Uri url, MethodType method, object data, string apiToken, RequestOptions options = null)
        {
            Method = method;
            Url = url;
            Data = data;
            Headers.Add("Accept-Encoding", "gzip");
            if (!string.IsNullOrEmpty(options?.Sudo))
            {
                Headers.Add("Sudo", options.Sudo);
            }

            if (apiToken != null)
            {
                // Use the 'Authorization: Bearer token' header as this provides flexibility to use
                // personal, project, group and OAuth tokens. The 'PRIVATE-TOKEN' header does not
                // provide OAuth token support.
                // Reference: https://docs.gitlab.com/ee/api/rest/#personalprojectgroup-access-tokens
                Headers.Add(HttpRequestHeader.Authorization, string.Concat("Bearer ", apiToken));
            }

            if (!string.IsNullOrEmpty(options?.UserAgent))
            {
                Headers.Add("User-Agent", options.UserAgent);
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

        public HttpResponseMessage GetResponse(RequestOptions options)
        {
            Func<HttpResponseMessage> getResponseImpl = () => GetResponseImpl(options);

            return getResponseImpl.Retry(options.ShouldRetry,
                options.RetryInterval,
                options.RetryCount,
                options.IsIncremental);
        }

        public Task<HttpResponseMessage> GetResponseAsync(RequestOptions options, CancellationToken cancellationToken)
        {
            Func<Task<HttpResponseMessage>> getResponseImpl = () => GetResponseImplAsync(options, cancellationToken);

            return getResponseImpl.RetryAsync(options.ShouldRetry,
                options.RetryInterval,
                options.RetryCount,
                options.IsIncremental);
        }

        private HttpResponseMessage GetResponseImpl(RequestOptions options)
        {
            var result = new GitLabRequestResult();
            try
            {
                result.Request = CreateRequest(options);
                result.Response = _httpClient.SendAsync(result.Request).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                result.Exception = ex is WebException wex && wex.Response is not null ?
                                   HandleWebException(wex) :
                                   ex;
            }

            options.ProcessGitLabRequestResult(result);

            return result.Exception is not null ? throw result.Exception : result.Response;
        }

        private async Task<HttpResponseMessage> GetResponseImplAsync(RequestOptions options, CancellationToken cancellationToken)
        {
            var result = new GitLabRequestResult();
            try
            {
                result.Request = CreateRequest(options);
                result.Response = await _httpClient.SendAsync(result.Request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                result.Exception = ex is WebException wex && wex.Response is not null ?
                                   HandleWebException(wex) :
                                   ex;
            }

            options.ProcessGitLabRequestResult(result);

            return result.Exception is not null ? throw result.Exception : result.Response;
        }

        private GitLabException HandleWebException(WebException ex)
        {
            using var errorResponse = (HttpWebResponse)ex.Response;
            string jsonString;
            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
            {
                jsonString = reader.ReadToEnd();
            }

            var errorMessage = ExtractErrorMessage(jsonString, out var errorDetails);
            var exceptionMessage =
                $"GitLab server returned an error ({errorResponse.StatusCode}): {errorMessage}. " +
                $"Original call: {Method} {Url}";

            if (JsonData != null)
            {
                exceptionMessage += $". With data {JsonData}";
            }

            return new GitLabException(exceptionMessage)
            {
                OriginalCall = Url,
                ErrorObject = errorDetails,
                StatusCode = errorResponse.StatusCode,
                ErrorMessage = errorMessage,
                MethodType = Method,
            };
        }

        private static HttpClient _httpClient = null;

        private HttpRequestMessage CreateRequest(RequestOptions options)
        {
            if (_httpClient == null)
            {
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip,
                    Proxy = options.Proxy,
                    UseProxy = options.Proxy != null,
                };
                _httpClient = new HttpClient(handler);
                _httpClient.Timeout = options.HttpClientTimeout;
            }

            var request = new HttpRequestMessage(new HttpMethod(Method.ToString().ToUpperInvariant()), Url);
            if (HasOutput)
            {
                if (FormData != null)
                {
                    AddFileData(request, options);
                }
                else if (UrlEncodedData != null)
                {
                    AddUrlEncodedData(request, options);
                }
                else if (JsonData != null)
                {
                    AddJsonData(request, options);
                }
            }
            else if (Method == MethodType.Put)
            {
                // request.ContentLength = 0;
            }

            return request;
        }

        private void AddJsonData(HttpRequestMessage request, RequestOptions options)
        {
            request.Content.Headers.ContentType.MediaType = "application/json";
            using var writer = new StreamWriter(options.GetRequestStream(request));
            writer.Write(JsonData);
            writer.Flush();
            writer.Close();
        }

        public void AddFileData(HttpRequestMessage request, RequestOptions options)
        {
            var boundary = $"--------------------------{DateTime.UtcNow.Ticks.ToStringInvariant()}";
            if (Data is not FormDataContent formData)
                return;
            request.Content.Headers.ContentType.MediaType = "multipart/form-data; boundary=" + boundary;
            using var uploadContent = new MultipartFormDataContent(boundary)
            {
                { new StreamContent(formData.Stream), "file", formData.Name },
            };

            uploadContent.CopyToAsync(options.GetRequestStream(request)).Wait();
        }

        public void AddUrlEncodedData(HttpRequestMessage request, RequestOptions options)
        {
            request.Content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            using var content = new FormUrlEncodedContent(UrlEncodedData.Values);
            content.CopyToAsync(options.GetRequestStream(request)).Wait();
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
    }
}
