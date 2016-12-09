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
        [DataMember(Name = "message")]
        public string Message;
#pragma warning restore 649
    }

    /// <summary>
    /// The requestor is typically used for a single call to gitlab.
    /// </summary>
    public class HttpRequestor : IHttpRequestor
    {
        private readonly MethodType _methodType;
        private object _data;

        private readonly string _apiToken;
        private readonly string _hostUrl;

        public HttpRequestor(string hostUrl, string apiToken, MethodType methodType)
        {
            _hostUrl = hostUrl.EndsWith("/") ? hostUrl.Replace("/$", "") : hostUrl;
            _apiToken = apiToken;
            _methodType = methodType;
        }

        public IHttpRequestor With(object data)
        {
            _data = data;
            return this;
        }

        public virtual T To<T>(string tailAPIUrl)
        {
            var result = default(T);
            Stream(tailAPIUrl, s => result = SimpleJson.DeserializeObject<T>(new StreamReader(s).ReadToEnd()));
            return result;
        }
        
        public Uri GetAPIUrl(string tailAPIUrl)
        {
            if (_apiToken != null)
            {
                tailAPIUrl = tailAPIUrl + (tailAPIUrl.IndexOf('?') > 0 ? '&' : '?') + "private_token=" + _apiToken;
            }

            if (!tailAPIUrl.StartsWith("/"))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }
            return UriFix.Build(_hostUrl + tailAPIUrl);
        }

        public Uri GetUrl(string tailAPIUrl)
        {
            if (!tailAPIUrl.StartsWith("/"))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }

            return UriFix.Build(_hostUrl + tailAPIUrl);
        }

        public virtual void Stream(string tailAPIUrl, Action<Stream> parser)
        {
            var req = SetupConnection(GetAPIUrl(tailAPIUrl));

            if (HasOutput())
            {
                SubmitData(req);
            }
            else if (_methodType == MethodType.Put)
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
                            JsonError jsonError;
                            try
                            {
                                jsonError = SimpleJson.DeserializeObject<JsonError>(jsonString);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("The remote server returned an error ({0}) with an empty response", errorResponse.StatusCode), ex);
                            }
                            throw new Exception(string.Format("The remote server returned an error ({0}): {1}", errorResponse.StatusCode, jsonError.Message));
                        }
                    }
                }
                else
                    throw wex;
            }
        }

        public virtual IEnumerable<T> GetAll<T>(string tailUrl)
        {
            return new Enumerable<T>(_apiToken, GetAPIUrl(tailUrl));
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

        private bool HasOutput() => (_methodType == MethodType.Delete || _methodType == MethodType.Post || _methodType == MethodType.Put) && _data != null;

        private WebRequest SetupConnection(Uri url) => SetupConnection(url, _methodType);

        private static WebRequest SetupConnection(Uri url, MethodType methodType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = methodType.ToString().ToUpperInvariant();
            request.Headers.Add("Accept-Encoding", "gzip");
            request.AutomaticDecompression = DecompressionMethods.GZip;

            return request;
        }
    }

    /// <summary>
    /// .Net framework has a bug which converts the escaped / into normal slashes
    /// This is not equivalent and fails when retrieving a project with its full name for example.
    /// There is an ugly workaround which involves reflection but it better than nothing.
    /// </summary>
    /// <remarks>
    /// http://stackoverflow.com/questions/5774183/how-to-make-system-uri-not-to-unescape-2f-slash-in-path
    /// </remarks>
    internal static class UriFix
    {
        public static Uri Build(string asString)
        {
            var uri = new Uri(asString);
            LeaveDotsAndSlashesEscaped(uri);
            return uri;
        }

        // System.UriSyntaxFlags is internal, so let's duplicate the flag privately
        private const int UnEscapeDotsAndSlashes = 0x2000000;
        private const int SimpleUserSyntax = 0x20000;

        private static void LeaveDotsAndSlashesEscaped(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            FieldInfo fieldInfo = uri.GetType().GetField("m_Syntax", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
                throw new MissingFieldException("'m_Syntax' field not found");

            object uriParser = fieldInfo.GetValue(uri);
            fieldInfo = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
                throw new MissingFieldException("'m_Flags' field not found");

            object uriSyntaxFlags = fieldInfo.GetValue(uriParser);

            // Clear the flag that we don't want
            uriSyntaxFlags = (int)uriSyntaxFlags & ~UnEscapeDotsAndSlashes;
            uriSyntaxFlags = (int)uriSyntaxFlags & ~SimpleUserSyntax;
            fieldInfo.SetValue(uriParser, uriSyntaxFlags);
        }
    }
}
