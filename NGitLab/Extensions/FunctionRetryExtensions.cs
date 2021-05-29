using System;
using System.Threading;

namespace NGitLab.Extensions
{
    /// <summary>
    ///  Helper methods to execute retry
    /// </summary>
    internal static class FunctionRetryExtensions
    {
        public static Action<string> Logger { get; set; }

        /// <summary>
        /// Do a retry a number of time on the received action if it fails
        /// </summary>
        public static T Retry<T>(this Func<T> action, Func<Exception, int, bool> predicate, TimeSpan waitTime, int maxRetryCount, bool useExponentialBackoff)
        {
            var retriesLeft = maxRetryCount;
            while (true)
            {
                try
                {
                    return action();
                }
                catch (Exception ex) when (retriesLeft > 0 && predicate(ex, retriesLeft))
                {
                    Logger?.Invoke($"{ex.Message} -> Internal Retry in {waitTime.TotalMilliseconds} ms ({maxRetryCount - retriesLeft + 1} of {maxRetryCount})...");

                    Thread.Sleep(waitTime);

                    if (useExponentialBackoff)
                    {
                        waitTime = waitTime.Add(waitTime);
                    }

                    retriesLeft--;
                }
            }
        }
    }
}
