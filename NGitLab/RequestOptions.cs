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
        /// Limits retries to safe HTTP requests (i.e. read-only and thus idempotent), such as GET and HEAD.
        /// </summary>
        /// <see href="https://developer.mozilla.org/en-US/docs/Glossary/Safe/HTTP"/>
        public bool RetrySafeRequestsOnly { get; set; } = true;

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
            return retryNumber > 0 && ex is GitLabException gitLabException && (int)gitLabException.StatusCode >= 500;
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
