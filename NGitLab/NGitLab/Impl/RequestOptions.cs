using System;
using System.Net;
using System.Web.Security;

namespace NGitLab.Impl

{
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

        public virtual bool ShouldRetry(Exception ex, int retryNumber)
        {
            return retryNumber > 0 && ex is WebException;
        }

        public static RequestOptions Default => new RequestOptions(0, TimeSpan.FromMilliseconds(0));
    }
}
