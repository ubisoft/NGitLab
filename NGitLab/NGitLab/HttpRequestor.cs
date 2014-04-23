using System;
using System.Collections.Generic;
using System.Net;

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

        public HttpRequestor With(string key, Object value) {
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
        
        try {
            return parse(req, type, instance);
        } catch (IOException e) {
            handleAPIError(e, req);
        }

        return null;
    }

    public List<T> GetAll<T> (string tailUrl) {
     var results = new List<T>();
     Iterator<T[]> iterator = asIterator(tailUrl, type);

        while (iterator.hasNext()) {
            T[] requests = iterator.next();

            if (requests.length > 0) {
                results.addAll(Arrays.asList(requests));
            }
        }
        return results;
    }

    public <T> Iterator<T> asIterator(final string tailApiUrl, final Class<T> type) {
        method("GET"); // Ensure we only use iterators for GET requests

        // Ensure that we don't submit any data and alert the user
        if (!_data.isEmpty()) {
            throw new IllegalStateException();
        }

        return new Iterator<T>() {
            T _next;
            URL _url;

            {
                try {
                    _url = _root.getAPIUrl(tailApiUrl);
                } catch (IOException e) {
                    throw new Error(e);
                }
            }

            public bool hasNext() {
                fetch();
                if (_next.getClass().isArray()) {
                    Object[] arr = (Object[]) _next;
                    return arr.length != 0;
                } else {
                    return _next != null;
                }
            }

            public T next() {
                fetch();
                T record = _next;

                if (record == null) {
                    throw new NoSuchElementException();
                }

                _next = null;
                return record;
            }

            public void remove() {
                throw new UnsupportedOperationException();
            }

            private void fetch() {
                if (_next != null) {
                    return;
                }

                if (_url == null) {
                    return;
                }

                try {
                    HttpURLConnection connection = SetupConnection(_url);
                    try {
                        _next = parse(connection, type, null);
                        assert _next != null;
                        findNextUrl(connection);
                    } catch (IOException e) {
                        handleAPIError(e, connection);
                    }
                } catch (IOException e) {
                    throw new Error(e);
                }
            }

            private void findNextUrl(HttpURLConnection connection) throws MalformedURLException {
                string url = _url.tostring();

                _url = null;
                /* Increment the page number for the url if a "page" property exists,
* otherwise, add the page property and increment it.
* The Gitlab API is not a compliant hypermedia REST api, so we use
* a naive implementation.
*/
                Pattern pattern = Pattern.compile("([&|?])page=(\\d+)");
                Matcher matcher = pattern.matcher(url);

                if (matcher.find()) {
                    int page = int.parseInt(matcher.group(2)) + 1;
                    _url = new URL(matcher.replaceAll(matcher.group(1) + "page=" + page));
                } else {
                    // Since the page query was not present, its safe to assume that we just
                    // currently used the first page, so we can default to page 2
                    _url = new URL(url + "&page=2");
                }
            }
        };
    }

    private void SubmitData(HttpWebRequest request){
        request.Headers.Add("Content-Type", "application/json");

        GitlabAPI.MAPPER.writeValue(connection.getOutputStream(), _data);
    }

    private bool HasOutput() {
        return _method == MethodType.Post || _method == MethodType.Put && _data.Count != 0;
    }

    private HttpWebRequest SetupConnection(Uri url){
        if (_root.IsIgnoreCertificateErrors) {
            ignoreCertificateErrors();
        }

        var request = HttpWebRequest.Create(url);
        request.Method = _method.Tostring().ToUpperInvariant();
        request.Headers.Add("Accept-Encoding", "gzip");
       
        return request;
    }

    private <T> T parse(HttpURLConnection connection, Class<T> type, T instance) throws IOException {
        InputStreamReader reader = null;
        try {
            reader = new InputStreamReader(wrapStream(connection, connection.getInputStream()), "UTF-8");
            string data = IOUtils.tostring(reader);

            if (type != null) {
                return GitlabAPI.MAPPER.readValue(data, type);
            } else if (instance != null) {
                return GitlabAPI.MAPPER.readerForUpdating(instance).readValue(data);
            } else {
                return null;
            }
        } catch (SSLHandshakeException e) {
            throw new SSLHandshakeException("You can disable certificate checking by setting ignoreCertificateErrors on HttpRequestor");
        } finally {
            IOUtils.closeQuietly(reader);
        }
    }

    private InputStream wrapStream(HttpURLConnection connection, InputStream inputStream) throws IOException {
        string encoding = connection.getContentEncoding();

        if (encoding == null || inputStream == null) {
            return inputStream;
        } else if (encoding.equals("gzip")) {
            return new GZIPInputStream(inputStream);
        } else {
            throw new UnsupportedOperationException("Unexpected Content-Encoding: " + encoding);
        }
    }

    private void handleAPIError(IOException e, HttpURLConnection connection) throws IOException {
        if (e instanceof FileNotFoundException) {
            throw e; // pass through 404 Not Found to allow the caller to handle it intelligently
        }

        InputStream es = wrapStream(connection, connection.getErrorStream());
        try {
            if (es != null) {
                throw (IOException) new IOException(IOUtils.tostring(es, "UTF-8")).initCause(e);
            } else {
                throw e;
            }
        } finally {
            IOUtils.closeQuietly(es);
        }
    }

    private void ignoreCertificateErrors() {
        System.Net.ServicePointManager.ServerCertificateValidationCallback
    }
}