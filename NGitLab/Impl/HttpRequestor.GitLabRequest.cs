using System;
using System.IO;
using System.Net;
using System.Net.Http;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
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

            private MethodType Method { get; }

            public WebHeaderCollection Headers { get; } = new WebHeaderCollection();

            private bool HasOutput
                => (Method == MethodType.Delete || Method == MethodType.Post || Method == MethodType.Put)
                    && Data != null;

            public GitLabRequest(Uri url, MethodType method, object data, string apiToken, string sudo = null)
            {
                Method = method;
                Url = url;
                Data = data;
                Headers.Add("Accept-Encoding", "gzip");
                if (!string.IsNullOrEmpty(sudo))
                {
                    Headers.Add("Sudo", sudo);
                }

                if (apiToken != null)
                {
                    Headers.Add("Private-Token", apiToken);
                }

                if (data is FormDataContent formData)
                {
                    FormData = formData;
                }
                else if (data != null)
                {
                    JsonData = SimpleJson.SerializeObject(data);
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

            private WebResponse GetResponseImpl(RequestOptions options)
            {
                try
                {
                    var request = CreateRequest(options);
                    return options.GetResponse(request);
                }
                catch (WebException wex)
                {
                    if (wex.Response == null)
                    {
                        throw;
                    }

                    using var errorResponse = (HttpWebResponse)wex.Response;
                    string jsonString;
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        jsonString = reader.ReadToEnd();
                    }

                    var errorMessage = ExtractErrorMessage(jsonString, out var parsedError);
                    var exceptionMessage =
                        $"GitLab server returned an error ({errorResponse.StatusCode}): {errorMessage}. " +
                        $"Original call: {Method} {Url}";

                    if (JsonData != null)
                    {
                        exceptionMessage += $". With data {JsonData}";
                    }

                    throw new GitLabException(exceptionMessage)
                    {
                        OriginalCall = Url,
                        ErrorObject = parsedError,
                        StatusCode = errorResponse.StatusCode,
                        ErrorMessage = errorMessage,
                        MethodType = Method,
                    };
                }
            }

            private HttpWebRequest CreateRequest(RequestOptions options)
            {
                var request = WebRequest.CreateHttp(Url);
                request.Method = Method.ToString().ToUpperInvariant();
                request.Accept = "application/json";
                request.Headers = Headers;
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.Timeout = (int)options.HttpClientTimeout.TotalMilliseconds;
                request.ReadWriteTimeout = (int)options.HttpClientTimeout.TotalMilliseconds;

                if (HasOutput)
                {
                    if (FormData != null)
                    {
                        AddFileData(request, options);
                    }
                    else if (JsonData != null)
                    {
                        AddJsonData(request, options);
                    }
                }
                else if (Method == MethodType.Put)
                {
                    request.ContentLength = 0;
                }

                return request;
            }

            private void AddJsonData(HttpWebRequest request, RequestOptions options)
            {
                request.ContentType = "application/json";

                using var writer = new StreamWriter(options.GetRequestStream(request));
                writer.Write(JsonData);
                writer.Flush();
                writer.Close();
            }

            public void AddFileData(HttpWebRequest request, RequestOptions options)
            {
                var boundary = $"--------------------------{DateTime.UtcNow.Ticks.ToStringInvariant()}";
                if (Data is not FormDataContent formData)
                    return;
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                using var uploadContent = new MultipartFormDataContent(boundary)
                {
                    { new StreamContent(formData.Stream), "file", formData.Name },
                };

                uploadContent.CopyToAsync(options.GetRequestStream(request)).Wait();
            }

            /// <summary>
            /// Parse the error that GitLab returns. GitLab returns structured errors but has a lot of them
            /// Here we try to be generic.
            /// </summary>
            /// <param name="json"></param>
            /// <param name="parsedError"></param>
            /// <returns></returns>
            private static string ExtractErrorMessage(string json, out JsonObject parsedError)
            {
                if (string.IsNullOrEmpty(json))
                {
                    parsedError = null;
                    return "Empty Response";
                }

                SimpleJson.TryDeserializeObject(json, out var errorObject);

                parsedError = errorObject as JsonObject;
                object messageObject = null;
                if (parsedError?.TryGetValue("message", out messageObject) != true)
                {
                    parsedError?.TryGetValue("error", out messageObject);
                }

                if (messageObject == null)
                {
                    return $"Error message cannot be parsed ({json})";
                }

                if (messageObject is string str)
                {
                    return str;
                }

                return SimpleJson.SerializeObject(messageObject);
            }
        }
    }
}
