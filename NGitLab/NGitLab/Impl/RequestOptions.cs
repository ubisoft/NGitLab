using System;
using System.Web.Security;

namespace NGitLab.Impl

{
    public class RequestOptions
    {
        public int RetryCount;

        public TimeSpan RetryInterval;

        public RequestOptions(int retryCount, TimeSpan retryInterval)
        {
            RetryCount = retryCount;
            RetryInterval = retryInterval;
        }

        public virtual bool ShouldRetry(Exception ex, int retryNumber)
        {
            return retryNumber > 0 && ex != null;
        }

        public static RequestOptions Default => new RequestOptions(0, TimeSpan.FromMilliseconds(0));
    }
}
