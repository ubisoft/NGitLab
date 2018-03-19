using System;
using System.Threading;
using NGitLab.Impl;

namespace NGitLab.Extensions
{
    /// <summary>
    ///  Helper methods to execute retry
    /// </summary>
    internal static class FunctionRetryExtensions
    {
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
                Thread.Sleep(interval);

                var nextInterval = isIncremental ? interval.Add(interval) : interval;
                return Retry(action, retryWhen, nextInterval, --retryNumber, isIncremental);
            }
        }
    }
}