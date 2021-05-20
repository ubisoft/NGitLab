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
        public static T Retry<T>(this Func<T> action, Func<Exception, int, bool> retryWhen, TimeSpan interval, int retryNumber, bool isIncremental)
        {
            try
            {
                return action();
            }
            catch (Exception ex) when (retryWhen(ex, retryNumber))
            {
                Logger?.Invoke($"{ex.Message} -> Internal Retry ({retryNumber - 1} attempts left)...");

                Thread.Sleep(interval);

                var nextInterval = isIncremental ? interval.Add(interval) : interval;
                return Retry(action, retryWhen, nextInterval, --retryNumber, isIncremental);
            }
        }
    }
}