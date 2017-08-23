using System;
using System.Diagnostics;

namespace NGitLab.Impl {
    [DebuggerStepThrough]
    public class Api {
        const string ApiNamespace = "/api/v4";
        readonly string hostUrl;
        public readonly string ApiToken;

        public Api(string hostUrl, string apiToken) {
            this.hostUrl = hostUrl.EndsWith("/") ? hostUrl.Replace("/$", "") : hostUrl;
            ApiToken = apiToken;
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

        public Uri GetApiUrl(string tailApiUrl) {
            //if (APIToken != null)
            //{
            //    tailAPIUrl = tailAPIUrl + (tailAPIUrl.IndexOf('?') > 0 ? '&' : '?') + "private_token=" + APIToken;
            //}

            if (!tailApiUrl.StartsWith("/"))
                tailApiUrl = "/" + tailApiUrl;
            return new Uri(hostUrl + ApiNamespace + tailApiUrl);
        }

        public Uri GetUrl(string tailApiUrl) {
            if (!tailApiUrl.StartsWith("/"))
                tailApiUrl = "/" + tailApiUrl;

            return new Uri(hostUrl + tailApiUrl);
        }
    }
}