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

        public static string AddKeysetPaginationParameter(string url, string orderBy, int? pageSize = null)
        {
            if (pageSize.HasValue)
            {
                url = AddParameter(url, "page_size", pageSize);
            }

            url = AddParameter(url, "order_by", orderBy);
            return AddParameter(url, "pagination", "keyset");
        }

        public static string AddOrderBy(string url, string orderBy = null, bool supportKeysetPagination = true)
        {
            if (supportKeysetPagination && (string.IsNullOrEmpty(orderBy) || string.Equals(orderBy, "id", StringComparison.Ordinal)))
            {
                return AddKeysetPaginationParameter(url, "id");
            }

            return AddParameter(url, "order_by", orderBy);
        }

        private static string AddParameterInternal(string url, string parameterName, string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
                return url;

            var @operator = !url.Contains("?") ? "?" : "&";
            var formattedValue = WebUtility.UrlEncode(stringValue);
            var parameter = $"{@operator}{parameterName}={formattedValue}";
            return url + parameter;
        }
    }
}
