using System;
using System.Net;

namespace NGitLab.Impl
{
    internal static class Utils
    {
        public static string AddParameter<T>(string url, string parameterName, T value)
        {
            if (Equals(value, null))
            {
                return url;
            }

            var @operator = !url.Contains("?") ? "?" : "&";
            var formattedValue = WebUtility.UrlEncode(value.ToString());
            var parameter = $"{@operator}{parameterName}={formattedValue}";
            return url + parameter;
        }

        public static string AddParameter(string url, string parameterName, DateTime? date)
        {
            if (Equals(date, null))
            {
                return url;
            }

            var value = date.Value;

            var @operator = !url.Contains("?") ? "?" : "&";
            var formattedValue = WebUtility.UrlEncode(value.ToString("o"));
            var parameter = $"{@operator}{parameterName}={formattedValue}";
            return url + parameter;
        }
    }
}
