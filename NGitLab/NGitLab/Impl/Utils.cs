using System;
using System.Net;

namespace NGitLab.Impl
{
    internal static class Utils
    {
        public static string AddParameter<T>(string url, string parameterName, T value)
        {
            return Equals(value, null) ? url : AddParameterInternal(url, parameterName, value.ToString());
        }

        public static string AddParameter(string url, string parameterName, DateTime? date)
        {
            return Equals(date, null) ? url : AddParameterInternal(url, parameterName, date.Value.ToString("o"));
        }

        private static string AddParameterInternal(string url, string parameterName, string stringValue)
        {
            var @operator = !url.Contains("?") ? "?" : "&";
            var formattedValue = WebUtility.UrlEncode(stringValue);
            var parameter = $"{@operator}{parameterName}={formattedValue}";
            return url + parameter;
        }
    }
}
