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

            string @operator = !url.Contains("?") ? "?" : "&";
            var formattedValue = WebUtility.UrlEncode(value.ToString());
            var parameter = $"{@operator}{parameterName}={formattedValue}";
            return url + parameter;
        }
    }
}
