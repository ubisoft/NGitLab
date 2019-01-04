using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;

namespace NGitLab.Impl
{
    [DataContract]
    internal class JsonError
    {
#pragma warning disable 649
        [DataMember(Name = "message")] public string Message;
#pragma warning restore 649
    }

    /// <summary>
    /// The requestor is typically used for a single call to gitlab.
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

        public HttpRequestor(string hostUrl, string apiToken, MethodType methodType) :
            this(hostUrl, apiToken, methodType, RequestOptions.Default)
        {
        }

        public HttpRequestor(string hostUrl, string apiToken, MethodType methodType, RequestOptions options)
        {
            _hostUrl = hostUrl.EndsWith("/", StringComparison.Ordinal) ? hostUrl.Replace("/$", "") : hostUrl;
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

        public virtual T To<T>(string tailAPIUrl)
        {
            var result = default(T);
            Stream(tailAPIUrl, s =>
            {
                var json = new StreamReader(s).ReadToEnd();
                result = SimpleJson.DeserializeObject<T>(json);
            });
            return result;
        }

        public Uri GetAPIUrl(string tailAPIUrl)
        {
            if (_apiToken != null)
            {
                tailAPIUrl = tailAPIUrl + (tailAPIUrl.IndexOf('?') > 0 ? '&' : '?') + "private_token=" + _apiToken;
            }

            if (!tailAPIUrl.StartsWith("/", StringComparison.Ordinal))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }

            return UriFix.Build(_hostUrl + tailAPIUrl);
        }

        public Uri GetUrl(string tailAPIUrl)
        {
            if (!tailAPIUrl.StartsWith("/", StringComparison.Ordinal))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }

            return UriFix.Build(_hostUrl + tailAPIUrl);
        }

        public virtual void Stream(string tailAPIUrl, Action<Stream> parser)
        {
            var request = new GitLabRequest(GetAPIUrl(tailAPIUrl), _methodType, _data);

            using (var response = request.GetResponse(_options))
            {
                if (parser != null)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        parser(stream);
                    }
                }
            }
        }

        public virtual IEnumerable<T> GetAll<T>(string tailUrl)
        {
            return new Enumerable<T>(_apiToken, GetAPIUrl(tailUrl), _options);
        }

        private class Enumerable<T> : IEnumerable<T>
        {
            private readonly string _apiToken;
            private readonly RequestOptions _options;
            private readonly Uri _startUrl;

            public Enumerable(string apiToken, Uri startUrl, RequestOptions options)
            {
                _apiToken = apiToken;
                _startUrl = startUrl;
                _options = options;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(_apiToken, _startUrl, _options);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class Enumerator : IEnumerator<T>
            {
                private readonly string _apiToken;
                private readonly RequestOptions _options;
                private Uri _nextUrlToLoad;
                private readonly List<T> _buffer = new List<T>();

                public Enumerator(string apiToken, Uri startUrl, RequestOptions options)
                {
                    _apiToken = apiToken;
                    _nextUrlToLoad = startUrl;
                    _options = options;
                }

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    if (_buffer.Count == 0)
                    {
                        if (_nextUrlToLoad == null)
                        {
                            return false;
                        }

                        var request = new GitLabRequest(_nextUrlToLoad, MethodType.Get, data: null);
                        request.Headers["PRIVATE-TOKEN"] = _apiToken;

                        using (var response = request.GetResponse(_options))
                        {
                            // <http://localhost:1080/api/v3/projects?page=2&per_page=0>; rel="next", <http://localhost:1080/api/v3/projects?page=1&per_page=0>; rel="first", <http://localhost:1080/api/v3/projects?page=2&per_page=0>; rel="last"
                            var link = response.Headers["Link"];

                            string[] nextLink = null;
                            if (!string.IsNullOrEmpty(link))
                            {
                                nextLink = link.Split(',')
                                   .Select(l => l.Split(';'))
                                   .FirstOrDefault(pair => pair[1].Contains("next"));
                            }

                            if (nextLink != null)
                            {
                                _nextUrlToLoad = new Uri(nextLink[0].Trim('<', '>', ' '));
                            }
                            else
                            {
                                _nextUrlToLoad = null;
                            }

                            var stream = response.GetResponseStream();
                            _buffer.AddRange(SimpleJson.DeserializeObject<T[]>(new StreamReader(stream).ReadToEnd()));
                        }

                        return _buffer.Count > 0;
                    }

                    if (_buffer.Count > 0)
                    {
                        _buffer.RemoveAt(0);
                        return (_buffer.Count > 0) ? true : MoveNext();
                    }

                    return false;
                }

                public void Reset()
                {
                    throw new NotSupportedException();
                }

                public T Current => _buffer[0];

                object IEnumerator.Current => Current;
            }
        }
    }

    /// <summary>
    /// .Net framework has a bug which converts the escaped / into normal slashes
    /// This is not equivalent and fails when retrieving a project with its full name for example.
    /// There is an ugly workaround which involves reflection but it better than nothing.
    /// </summary>
    /// <remarks>
    /// http://stackoverflow.com/questions/2320533/system-net-uri-with-urlencoded-characters/
    /// </remarks>
    internal static class UriFix
    {
        static UriFix()
        {
            LeaveDotsAndSlashesEscaped();
        }

        public static Uri Build(string asString)
        {
            return new Uri(asString);
        }

        public static void LeaveDotsAndSlashesEscaped()
        {
#if NET45
            var getSyntaxMethod =
                typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            if (getSyntaxMethod == null)
            {
                throw new MissingMethodException("UriParser", "GetSyntax");
            }

            var uriParser = getSyntaxMethod.Invoke(null, new object[] { "https" });

            var setUpdatableFlagsMethod =
                uriParser.GetType().GetMethod("SetUpdatableFlags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (setUpdatableFlagsMethod == null)
            {
                throw new MissingMethodException("UriParser", "SetUpdatableFlags");
            }

            setUpdatableFlagsMethod.Invoke(uriParser, new object[] { 0 });
#endif
        }
    }
}
