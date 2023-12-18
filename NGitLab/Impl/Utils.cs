using System;
using System.Globalization;
using System.Net;

namespace NGitLab.Impl
{
    internal static class Utils
    {
        public static string AddParameter<T>(string url, string parameterName, T value)
        {
            return Equals(value, null) ? url : AddParameterInternal(url, parameterName, value.ToString());
        }

        public static string AddParameter(string url, string parameterName, int? value)
        {
            if (!value.HasValue)
                return url;

            return AddParameterInternal(url, parameterName, value?.ToString(CultureInfo.InvariantCulture));
        }

        public static string AddParameter(string url, string parameterName, DateTime? date)
        {
            return Equals(date, null) ? url : AddParameterInternal(url, parameterName, date.Value.ToString("O"));
        }

        public static string AddParameter(string url, string parameterName, int[] values)
        {
            return Equals(values, null) ? url : AddParameterInternal(url, parameterName, string.Join(",", values));
        }

        public static string AddArrayParameter(string url, string parameterName, string[] values)
        {
            if (Equals(values, null))
            {
                return url;
            }

            foreach (var value in values)
            {
                url = AddParameterInternal(url, $"{parameterName}[]", value);
            }

            return url;
        }

        public static string AddOrderBy(string url, string orderBy = null, bool supportKeysetPagination = true)
        {
            if (supportKeysetPagination && (string.IsNullOrEmpty(orderBy) || string.Equals(orderBy, "id", StringComparison.Ordinal)))
            {
                return AddKeysetPaginationParameter(url);
            }

            return AddParameter(url, "order_by", orderBy);

            static string AddKeysetPaginationParameter(string url)
            {
                url = AddParameter(url, "order_by", "id");
                return AddParameter(url, "pagination", "keyset");
            }
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
