using System;
using System.Diagnostics;

namespace NGitLab.Impl
{
    [DebuggerStepThrough]
    public class API
    {
        // defined return codes for v3.
        public const string OK = "200";
        public const string CREATED = "201";
        public const string BAD_REQUEST = "400";
        public const string UNAUTHORIZED = "401";
        public const string FORBIDDEN = "403";
        public const string NOT_FOUND = "404";
        public const string METHOD_NOT_ALLOWED = "405";
        public const string CONFLICT = "409";
        public const string UNPROCESSABLE = "422";
        public const string SERVER_ERROR = "500";

        public readonly string APIToken;
        private readonly string _hostUrl;
        private const string APINamespace = "/api/v3";

        public API(string hostUrl, string apiToken)
        {
            _hostUrl = hostUrl.EndsWith("/") ? hostUrl.Replace("/$", "") : hostUrl;
            APIToken = apiToken;
        }
        
        public HttpRequestor Get()
        {
            return new HttpRequestor(this, MethodType.Get);
        }

        public HttpRequestor Post()
        {
            return new HttpRequestor(this, MethodType.Post);
        }

        public HttpRequestor Put()
        {
            return new HttpRequestor(this, MethodType.Put);
        }
        
        public HttpRequestor Delete()
        {
            return new HttpRequestor(this, MethodType.Delete);
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