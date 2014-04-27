using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
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
            throw new NotImplementedException();
        }

//        public List<T> GetAll<T>(string tailUrl)
//        {
//            var results = new List<T>();
//            Iterator<T[]> iterator = asIterator(tailUrl, type);

//            while (iterator.hasNext())
//            {
//                T[] requests = iterator.next();

//                if (requests.length > 0)
//                {
//                    results.addAll(Arrays.asList(requests));
//                }
//            }
//            return results;
//        }

//        public  <
//        private T 
//    >
//        private Iterator<T> asIterator(final 
//        private string tailApiUrl, final
//        private Class<T> type 
//    )
//    {
//        method("GET"); // Ensure we only use iterators for GET requests

//        // Ensure that we don't submit any data and alert the user
//        if (!_data.isEmpty())
//        {
//            throw new IllegalStateException();
//        }

//        return new Iterator<T>()
//        {
//            T _next;
//    URL _url;

//{
//    try {
//    _url = _root.getAPIUrl(tailApiUrl);
//        } catch
//        (IOException
//        e)
//        {
//            throw new Error(e);
//        }
//    }

//    public
//        bool hasNext 
//        ()
//        {
//            fetch();
//            if (_next.getClass().isArray())
//            {
//                Object[] arr = (Object[]) _next;
//                return arr.length != 0;
//            }
//            else
//            {
//                return _next != null;
//            }
//        }

//    public
//        T next 
//        ()
//        {
//            fetch();
//            T record = _next;

//            if (record == null)
//            {
//                throw new NoSuchElementException();
//            }

//            _next = null;
//            return record;
//        }

//    public
//        void remove 
//        ()
//        {
//            throw new UnsupportedOperationException();
//        }

//    private
//        void fetch 
//        ()
//        {
//            if (_next != null)
//            {
//                return;
//            }

//            if (_url == null)
//            {
//                return;
//            }

//            try
//            {
//                HttpURLConnection connection = SetupConnection(_url);
//                try
//                {
//                    _next = parse(connection, type, null);
//                    assert
//                    _next != null;
//                    findNextUrl(connection);
//                }
//                catch (IOException e)
//                {
//                    handleAPIError(e, connection);
//                }
//            }
//            catch (IOException e)
//            {
//                throw new Error(e);
//            }
//        }

//    private
//        void findNextUrl 
//        (HttpURLConnection
//        connection)
//        throws
//        MalformedURLException
//        {
//            string url = _url.tostring();

//            _url = null;
//            /* Increment the page number for the url if a "page" property exists,
//* otherwise, add the page property and increment it.
//* The Gitlab API is not a compliant hypermedia REST api, so we use
//* a naive implementation.
//*/
//            Pattern pattern = Pattern.compile("([&|?])page=(\\d+)");
//            Matcher matcher = pattern.matcher(url);

//            if (matcher.find())
//            {
//                int page = int.parseInt(matcher.group(2)) + 1;
//                _url = new URL(matcher.replaceAll(matcher.group(1) + "page=" + page));
//            }
//            else
//            {
//                // Since the page query was not present, its safe to assume that we just
//                // currently used the first page, so we can default to page 2
//                _url = new URL(url + "&page=2");
//            }
//        }
//    }
//        ;
//    }

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
            if (_root.IsIgnoreCertificateErrors)
            {
                IgnoreCertificateErrors();
            }

            var request = WebRequest.Create(url);
            request.Method = _method.ToString().ToUpperInvariant();
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;
        }

        private static void IgnoreCertificateErrors()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, errors) => true;
        }
    }
}