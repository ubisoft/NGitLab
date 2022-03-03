using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Impl.Json;

namespace NGitLab.Impl
{
    /// <summary>
    /// The requestor is typically used for a single call to GitLab.
    /// </summary>
    public partial class HttpRequestor : IHttpRequestor
    {
        private readonly RequestOptions _options;
        private readonly MethodType _methodType;
        private object _data;

        private readonly string _apiToken;
        private readonly string _hostUrl;

        static HttpRequestor()
        {
            // By default only Sssl and Tls 1.0 is enabled with .NET 4.5
            // We add Tls 1.2 and Tls 1.2 without affecting the other values in case new protocols are added in the future
            // (see https://stackoverflow.com/questions/28286086/default-securityprotocol-in-net-4-5)
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public HttpRequestor(string hostUrl, string apiToken, MethodType methodType)
            : this(hostUrl, apiToken, methodType, RequestOptions.Default)
        {
        }

        public HttpRequestor(string hostUrl, string apiToken, MethodType methodType, RequestOptions options)
        {
            _hostUrl = hostUrl.EndsWith("/", StringComparison.Ordinal) ? hostUrl.Replace("/$", string.Empty) : hostUrl;
            _apiToken = apiToken;
            _methodType = methodType;
            _options = options;
        }

        public IHttpRequestor With(object data)
        {
            _data = data;
            return this;
        }

        public virtual void Execute(string tailAPIUrl)
        {
            Stream(tailAPIUrl, parser: null);
        }

        public virtual Task ExecuteAsync(string tailAPIUrl, CancellationToken cancellationToken)
        {
            return StreamAsync(tailAPIUrl, parser: null, cancellationToken);
        }

        public virtual T To<T>(string tailAPIUrl)
        {
            var result = default(T);
            Stream(tailAPIUrl, s =>
            {
                var json = new StreamReader(s).ReadToEnd();
                result = Serializer.Deserialize<T>(json);
            });
            return result;
        }

        public virtual async Task<T> ToAsync<T>(string tailAPIUrl, CancellationToken cancellationToken)
        {
            var result = default(T);
            await StreamAsync(tailAPIUrl, async s =>
            {
                var json = await new StreamReader(s).ReadToEndAsync().ConfigureAwait(false);
                result = Serializer.Deserialize<T>(json);
            }, cancellationToken).ConfigureAwait(false);
            return result;
        }

        public Uri GetAPIUrl(string tailAPIUrl)
        {
            if (!tailAPIUrl.StartsWith("/", StringComparison.Ordinal))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }

            if (!tailAPIUrl.StartsWith("/api", StringComparison.Ordinal))
            {
                tailAPIUrl = "/api/v4" + tailAPIUrl;
            }

            return new Uri(_hostUrl + tailAPIUrl);
        }

        public Uri GetUrl(string tailAPIUrl)
        {
            if (!tailAPIUrl.StartsWith("/", StringComparison.Ordinal))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }

            return new Uri(_hostUrl + tailAPIUrl);
        }

        public virtual void Stream(string tailAPIUrl, Action<Stream> parser)
        {
            var request = new GitLabRequest(GetAPIUrl(tailAPIUrl), _methodType, _data, _apiToken, _options.Sudo);

            using var response = request.GetResponse(_options);
            if (parser != null)
            {
                using var stream = response.GetResponseStream();
                parser(stream);
            }
        }

        public virtual async Task StreamAsync(string tailAPIUrl, Func<Stream, Task> parser, CancellationToken cancellationToken)
        {
            var request = new GitLabRequest(GetAPIUrl(tailAPIUrl), _methodType, _data, _apiToken, _options.Sudo);

            using var response = await request.GetResponseAsync(_options, cancellationToken).ConfigureAwait(false);
            if (parser != null)
            {
                using var stream = response.GetResponseStream();
                await parser(stream).ConfigureAwait(false);
            }
        }

        public virtual IEnumerable<T> GetAll<T>(string tailUrl)
        {
            return new Enumerable<T>(_apiToken, GetAPIUrl(tailUrl), _options);
        }

        public virtual GitLabCollectionResponse<T> GetAllAsync<T>(string tailUrl)
        {
            return new Enumerable<T>(_apiToken, GetAPIUrl(tailUrl), _options);
        }

        internal sealed class Enumerable<T> : GitLabCollectionResponse<T>
        {
            private readonly string _apiToken;
            private readonly RequestOptions _options;
            private readonly Uri _startUrl;

            internal Enumerable(string apiToken, Uri startUrl, RequestOptions options)
            {
                _apiToken = apiToken;
                _startUrl = startUrl;
                _options = options;
            }

            public override async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                var nextUrlToLoad = _startUrl;
                while (nextUrlToLoad != null)
                {
                    string responseText;
                    var request = new GitLabRequest(nextUrlToLoad, MethodType.Get, data: null, _apiToken, _options.Sudo);
                    using (var response = await request.GetResponseAsync(_options, cancellationToken).ConfigureAwait(false))
                    {
                        nextUrlToLoad = GetNextPageUrl(response);

                        var stream = response.GetResponseStream();
                        using var streamReader = new StreamReader(stream);
                        responseText = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                    }

                    var deserialized = Serializer.Deserialize<T[]>(responseText);
                    foreach (var item in deserialized)
                        yield return item;
                }
            }

            public override IEnumerator<T> GetEnumerator()
            {
                var nextUrlToLoad = _startUrl;
                while (nextUrlToLoad != null)
                {
                    string responseText;
                    var request = new GitLabRequest(nextUrlToLoad, MethodType.Get, data: null, _apiToken, _options.Sudo);
                    using (var response = request.GetResponse(_options))
                    {
                        nextUrlToLoad = GetNextPageUrl(response);

                        using var stream = response.GetResponseStream();
                        using var streamReader = new StreamReader(stream);
                        responseText = streamReader.ReadToEnd();
                    }

                    var deserialized = Serializer.Deserialize<T[]>(responseText);
                    foreach (var item in deserialized)
                        yield return item;
                }
            }

            private static Uri GetNextPageUrl(WebResponse response)
            {
                // <http://localhost:1080/api/v3/projects?page=2&per_page=0>; rel="next", <http://localhost:1080/api/v3/projects?page=1&per_page=0>; rel="first", <http://localhost:1080/api/v3/projects?page=2&per_page=0>; rel="last"
                var link = response.Headers["Link"] ?? response.Headers["Links"];

                string[] nextLink = null;
                if (!string.IsNullOrEmpty(link))
                {
                    nextLink = link.Split(',')
                       .Select(l => l.Split(';'))
                       .FirstOrDefault(pair => pair[1].Contains("next"));
                }

                return nextLink != null ? new Uri(nextLink[0].Trim('<', '>', ' ')) : null;
            }
        }
    }
}
