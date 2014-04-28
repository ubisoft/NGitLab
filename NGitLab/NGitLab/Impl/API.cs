using System;
using System.Diagnostics;

namespace NGitLab.Impl
{
    public class API
    {
        public readonly string APIToken;
        private readonly string _hostUrl;
        private const string APINamespace = "/api/v3";

        public API(string hostUrl, string apiToken)
        {
            _hostUrl = hostUrl.EndsWith("/") ? hostUrl.Replace("/$", "") : hostUrl;
            APIToken = apiToken;
        }
        
        [DebuggerStepThrough]
        public HttpRequestor Retrieve()
        {
            return new HttpRequestor(this);
        }

        [DebuggerStepThrough]
        public HttpRequestor Dispatch()
        {
            return new HttpRequestor(this).Method(MethodType.Post);
        }

        public Uri GetAPIUrl(string tailAPIUrl)
        {
            if (APIToken != null)
            {
                tailAPIUrl = tailAPIUrl + (tailAPIUrl.IndexOf('?') > 0 ? '&' : '?') + "private_token=" + APIToken;
            }

            if (!tailAPIUrl.StartsWith("/"))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }
            return new Uri(_hostUrl + APINamespace + tailAPIUrl);
        }

        public Uri GetUrl(string tailAPIUrl)
        {
            if (!tailAPIUrl.StartsWith("/"))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }

            return new Uri(_hostUrl + tailAPIUrl);
        }
    }
}