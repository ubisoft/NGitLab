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
            return retryNumber > 0 && (ex is GitLabException gitLabException && (int)gitLabException.StatusCode >= 500);
        }

        public static RequestOptions Default => new RequestOptions(0, TimeSpan.FromMilliseconds(0));

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
