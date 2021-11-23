using System;
using System.Threading;
using System.Threading.Tasks;

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
                    var currentRetry = maxRetryCount - retriesLeft + 1;
                    Logger?.Invoke($"{ex.Message} -> Internal Retry in {waitTime.TotalMilliseconds.ToStringInvariant()} ms ({currentRetry.ToStringInvariant()} of {maxRetryCount.ToStringInvariant()})...");

                    Thread.Sleep(waitTime);

                    if (useExponentialBackoff)
                    {
                        waitTime = waitTime.Add(waitTime);
                    }

                    retriesLeft--;
                }
            }
        }

        /// <summary>
        /// Do a retry a number of time on the received action if it fails
        /// </summary>
        public static async Task<T> RetryAsync<T>(this Func<Task<T>> action, Func<Exception, int, bool> predicate, TimeSpan waitTime, int maxRetryCount, bool useExponentialBackoff)
        {
            var retriesLeft = maxRetryCount;
            while (true)
            {
                try
                {
                    return await action().ConfigureAwait(false);
                }
                catch (Exception ex) when (retriesLeft > 0 && predicate(ex, retriesLeft))
                {
                    var currentRetry = maxRetryCount - retriesLeft + 1;
                    Logger?.Invoke($"{ex.Message} -> Internal Retry in {waitTime.TotalMilliseconds.ToStringInvariant()} ms ({currentRetry.ToStringInvariant()} of {maxRetryCount.ToStringInvariant()})...");

                    await Task.Delay(waitTime).ConfigureAwait(false);

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
