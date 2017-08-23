using System;
using System.Diagnostics;

namespace NGitLab.Impl {
    [DebuggerStepThrough]
    public class API {
        const string APINamespace = "/api/v4";
        readonly string _hostUrl;
        public readonly string APIToken;

        public API(string hostUrl, string apiToken) {
            _hostUrl = hostUrl.EndsWith("/") ? hostUrl.Replace("/$", "") : hostUrl;
            APIToken = apiToken;
        }

        public HttpRequestor Get() {
            return new HttpRequestor(this, MethodType.Get);
        }

        public HttpRequestor Post() {
            return new HttpRequestor(this, MethodType.Post);
        }

        public HttpRequestor Put() {
            return new HttpRequestor(this, MethodType.Put);
        }

        public HttpRequestor Delete() {
            return new HttpRequestor(this, MethodType.Delete);
        }

        public Uri GetAPIUrl(string tailAPIUrl) {
            //if (APIToken != null)
            //{
            //    tailAPIUrl = tailAPIUrl + (tailAPIUrl.IndexOf('?') > 0 ? '&' : '?') + "private_token=" + APIToken;
            //}

            if (!tailAPIUrl.StartsWith("/"))
                tailAPIUrl = "/" + tailAPIUrl;
            return new Uri(_hostUrl + APINamespace + tailAPIUrl);
        }

        public Uri GetUrl(string tailAPIUrl) {
            if (!tailAPIUrl.StartsWith("/"))
                tailAPIUrl = "/" + tailAPIUrl;

            return new Uri(_hostUrl + tailAPIUrl);
        }
    }
}