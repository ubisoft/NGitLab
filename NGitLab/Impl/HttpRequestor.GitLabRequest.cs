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
            result.Request = CreateRequest(options);
            options.ProcessGitLabRequestResult(result).GetAwaiter().GetResult();
            return result.Exception is not null ? throw result.Exception : result.Response;
        }

        private async Task<HttpResponseMessage> GetResponseImplAsync(RequestOptions options, CancellationToken cancellationToken)
        {
            var result = new GitLabRequestResult();
            result.Request = CreateRequest(options);
            await options.ProcessGitLabRequestResult(result).ConfigureAwait(false);
            return result.Exception is not null ? throw result.Exception : result.Response;
        }

        private HttpRequestMessage CreateRequest(RequestOptions options)
        {
            var request = new HttpRequestMessage(new HttpMethod(Method.ToString().ToUpperInvariant()), Url);
            // Copy headers from Headers property to request
            if (Headers != null)
            {
                foreach (var key in Headers.AllKeys)
                {
                    var values = Headers.GetValues(key);
                    if (!request.Headers.TryAddWithoutValidation(key, values))
                    {
                        // If it fails to add as a request header, it might be a content header
                        if (request.Content != null)
                        {
                            request.Content.Headers.TryAddWithoutValidation(key, values);
                        }
                    }
                }
            }

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
            request.Content = new StringContent(JsonData, System.Text.Encoding.UTF8, "application/json");
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
