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
            var fullUrl = GetAPIUrl(tailAPIUrl);
            var req = SetupConnection(fullUrl);

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
                        string jsonString;
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            jsonString = reader.ReadToEnd();
                        }

                        JsonObject parsedError;
                        var errorMessage = ExtractErrorMessage(jsonString, out parsedError);
                        var exceptionMessage =
                            $"GitLab server returned an error ({errorResponse.StatusCode}): {errorMessage}. " +
                            $"Original call: {_methodType} {fullUrl}";

                        if (_data != null)
                        {
                            exceptionMessage += $". With data {SimpleJson.SerializeObject(_data)}";
                        }

                        throw new GitLabException(exceptionMessage)
                        {
                            OriginalCall = fullUrl,
                            ErrorObject = parsedError,
                            StatusCode = errorResponse.StatusCode,
                            ErrorMessage = errorMessage,
                        };
                    }
                }
                else
                    throw wex;
            }
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

            object errorObject;
            SimpleJson.TryDeserializeObject(json, out errorObject);

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

            if (messageObject is string)
            {
                return (string)messageObject;
            }

            return SimpleJson.SerializeObject(messageObject);
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
        }
    }
}
