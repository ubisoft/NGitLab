using System;

namespace NGitLab.Tests.Extensions
{
    public static class TestClassExtensions
    {
        public static T ExecuteWithFallbacks<T>(this object _, Func<IGitLabClient, T> func)
        {
            try
            {
                return func.Invoke(Initialize.GitLabClient);
            }
            catch
            {
                // ignored, fallback to the first host front
            }

            try
            {
                return func.Invoke(Initialize.GitLabClientFront1);
            }
            catch
            {
                // ignored, fallback to the second host front
            }

            return func.Invoke(Initialize.GitLabClientFront2);
        }
    }
}