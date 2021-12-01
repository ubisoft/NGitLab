using System;
using NUnit.Framework;

namespace NGitLab.Tests
{
    internal static class Utils
    {
        internal static readonly bool RunningInCiEnvironment =
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITLAB_CI"));

        /// <summary>
        /// Fails a test when executed in a GitLab CI environment, but marks it inconclusive otherwise.
        /// </summary>
        /// <param name="message">Error/inconclusive message</param>
        public static void FailInCiEnvironment(string message)
        {
            if (RunningInCiEnvironment)
            {
                Assert.Fail(message);
            }
            else
            {
                Assert.Inconclusive(message);
            }
        }
    }
}
