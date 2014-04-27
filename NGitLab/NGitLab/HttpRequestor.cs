using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace NGitLab
{
    public class HttpRequestor
    {
        private readonly API _root;
        private MethodType _method = MethodType.Get; // Default to GET requests
        private readonly Dictionary<string, Object> _data = new Dictionary<string, Object>();

        public enum MethodType
        {
            Get,
            Put,
            Post,
            Patch,
            Delete,
            Head,
            Options,
            Trace
        }

        public HttpRequestor(API root)
        {
            _root = root;
        }

        public HttpRequestor Method(MethodType method)
        {
            _method = method;
            return this;
        }

        public HttpRequestor With(string key, Object value)
        {
            if (value != null && key != null)
            {
                _data[key] = value;
            }
            return this;
        }

        public T To<T>(string tailAPIUrl, T instance = default(T))
        {
            var req = SetupConnection(_root.GetAPIUrl(tailAPIUrl));

            if (HasOutput())
            {
                SubmitData(req);
            }
            else if (_method == MethodType.Put)
            {
                req.Headers.Add("Content-Length", "0");
            }

            using (var response = req.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    return GetSerializer().Deserialize<T>(new JsonTextReader(new StreamReader(stream)));
                }
            }
        }

        private static JsonSerializer GetSerializer()
        {
            return new JsonSerializer()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
        }

        public IEnumerable<T> GetAll<T>(string tailUrl)
        {
            return new Enumerable<T>(_root.APIToken, _root.GetAPIUrl(tailUrl));
        }

        private class Enumerable<T> : IEnumerable<T>
        {
            private readonly string _apiToken;
            private readonly Uri _startUrl;

            public Enumerable(string apiToken, Uri startUrl)
            {
                _apiToken = apiToken;
                _startUrl = startUrl;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator<T>(_apiToken, _startUrl);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class Enumerator<T> : IEnumerator<T>
            {
                private readonly string _apiToken;
                private Uri _nextUrlToLoad;
                private readonly List<T> _buffer = new List<T>();

                private bool _finished;

                public Enumerator(string apiToken, Uri startUrl)
                {
                    _apiToken = apiToken;
                    _nextUrlToLoad = startUrl;
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

                        var request = SetupConnection(_nextUrlToLoad, MethodType.Get);
                        request.Headers["PRIVATE-TOKEN"] = _apiToken;

                        using (var response = request.GetResponse())
                        {
                            // <http://localhost:1080/api/v3/projects?page=2&per_page=0>; rel="next", <http://localhost:1080/api/v3/projects?page=1&per_page=0>; rel="first", <http://localhost:1080/api/v3/projects?page=2&per_page=0>; rel="last"
                            var nextLink = response.Headers["Link"].Split(',')
                                .Select(l => l.Split(';'))
                                .FirstOrDefault(pair => pair[1].Contains("next"));

                            if (nextLink != null)
                            {
                                _nextUrlToLoad = new Uri(nextLink[0].Trim('<', '>', ' '));
                            }
                            else
                            {
                                _nextUrlToLoad = null;
                            }

                            var stream = response.GetResponseStream();
                            _buffer.AddRange(GetSerializer().Deserialize<T[]>(new JsonTextReader(new StreamReader(stream))));
                        }

                        return _buffer.Count > 0;
                    }

                    if (_buffer.Count > 0)
                    {
                        _buffer.RemoveAt(0);
                        return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    throw new NotImplementedException();
                }

                public T Current
                {
                    get
                    {
                        return _buffer[0];
                    }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }
            }
        }

        private void SubmitData(WebRequest request)
        {
            request.Headers.Add("Content-Type", "application/json");

            using (var stream = request.GetRequestStream())
            {
                GetSerializer().Serialize(new StreamWriter(stream), _data);
            }
        }

        private bool HasOutput()
        {
            return _method == MethodType.Post || _method == MethodType.Put && _data.Count != 0;
        }

        private WebRequest SetupConnection(Uri url)
        {
            return SetupConnection(url, _method);
        }

        private static WebRequest SetupConnection(Uri url, MethodType methodType)
        {
            var request = WebRequest.Create(url);
            request.Method = methodType.ToString().ToUpperInvariant();
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;
        }
    }
}