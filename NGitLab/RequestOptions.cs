using System;
using System.Net;

namespace NGitLab
{
    /// <summary>
    /// Allows to override how NGitLab executes queries to the GitLab server,
    /// each http call goes in that handler.
    /// </summary>
    public class RequestOptions
    {
        public int RetryCount { get; set; }

        public TimeSpan RetryInterval { get; set; }

        public bool IsIncremental { get; set; }

        /// <summary>
        /// ID or case-insensitive username of the user to impersonate, if any
        /// </summary>
        public string Sudo { get; set; }

        /// <summary>
        /// Configure the default client side timeout when calling GitLab.
        /// GitLab exposes some end points which are really slow so
        /// the default we use is larger than the default 100 seconds of .net
        /// </summary>
        public TimeSpan HttpClientTimeout { get; set; } = TimeSpan.FromMinutes(5);

        public RequestOptions(int retryCount, TimeSpan retryInterval, bool isIncremental = true)
        {
            RetryCount = retryCount;
            RetryInterval = retryInterval;
            IsIncremental = isIncremental;
        }

        /// <summary>
        /// By default, true if there is still retries left.
        /// </summary>
        public virtual bool ShouldRetry(Exception ex, int retryNumber)
        {
            if (retryNumber < 1)
                return false;

            if (!(ex is GitLabException gitLabException))
                return false;

            // For requests that are potentially NOT Safe/Idempotent, do not retry
            // See https://developer.mozilla.org/en-US/docs/Glossary/Safe/HTTP
            // If there is no HTTP method info, carry on to the next condition below.
            if (gitLabException.MethodType.HasValue &&
                gitLabException.MethodType != Impl.MethodType.Get &&
                gitLabException.MethodType != Impl.MethodType.Head)
                return false;

            // Same as what are considered Transient HTTP StatusCodes in Polly's HttpPolicyExtensions
            // https://github.com/App-vNext/Polly.Extensions.Http/blob/69fd292bc603cb3032e57b028522737255f03a49/src/Polly.Extensions.Http/HttpPolicyExtensions.cs#L14
            return gitLabException.StatusCode >= HttpStatusCode.InternalServerError ||
                   gitLabException.StatusCode == HttpStatusCode.RequestTimeout;
        }

        public static RequestOptions Default => new RequestOptions(0, TimeSpan.Zero);

        /// <summary>
        /// Allows to monitor the web requests from the caller library, for example
        /// to log the request duration and debug the library.
        /// </summary>
        public virtual WebResponse GetResponse(HttpWebRequest request)
        {
            return request.GetResponse();
        }
    }
}
