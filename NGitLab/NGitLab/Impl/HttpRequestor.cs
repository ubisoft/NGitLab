using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace NGitLab.Impl
{
    [DataContract]
    internal class JsonError
    {
#pragma warning disable 649
        [DataMember(Name = "message")]
        public string Message;
#pragma warning restore 649
    }

    public class HttpRequestor
    {
        private readonly API _root;
        private readonly MethodType _method; // Default to GET requests
        private object _data;

        public HttpRequestor(API root, MethodType method)
        {
            _root = root;
            _method = method;
        }

        public HttpRequestor With(object data)
        {
            _data = data;
            return this;
        }

        public T To<T>(string tailAPIUrl)
        {
            var result = default(T);
            Stream(tailAPIUrl, s => result = SimpleJson.DeserializeObject<T>(new StreamReader(s).ReadToEnd()));
            return result;
        }

        public void Stream(string tailAPIUrl, Action<Stream> parser)
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

            try
            {
                using (var response = req.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        parser(stream);
                    }
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string jsonString = reader.ReadToEnd();
                            var jsonError = SimpleJson.DeserializeObject<JsonError>(jsonString);
                            throw new Exception(string.Format("The remote server returned an error ({0}): {1}", errorResponse.StatusCode, jsonError.Message));
                        }
                    }
                }
                else
                    throw wex;
            }

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
                return new Enumerator(_apiToken, _startUrl);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class Enumerator : IEnumerator<T>
            {
                private readonly string _apiToken;
                private Uri _nextUrlToLoad;
                private readonly List<T> _buffer = new List<T>();

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
                            var link = response.Headers["Link"];

                            string[] nextLink = null;
                            if (string.IsNullOrEmpty(link) == false)
                                nextLink = link.Split(',')
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
            request.ContentType = "application/json";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                var data = SimpleJson.SerializeObject(_data);
                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
        }

        private bool HasOutput()
        {
            return (_method == MethodType.Delete || _method == MethodType.Post || _method == MethodType.Put) && _data != null;
        }

        private WebRequest SetupConnection(Uri url)
        {
            return SetupConnection(url, _method);
        }

        private static WebRequest SetupConnection(Uri url, MethodType methodType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = methodType.ToString().ToUpperInvariant();
            request.Headers.Add("Accept-Encoding", "gzip");
            request.AutomaticDecompression = DecompressionMethods.GZip;

            return request;
        }
    }
}