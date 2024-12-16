using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace NGitLab.Impl;

internal static class Utils
{
    public static string AddParameter<T>(string url, string parameterName, T value)
    {
        if (value is null)
            return url;

        var valueString = value.ToString();
        var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        if (type.IsEnum)
        {
            var enumField = type.GetFields().FirstOrDefault(f => string.Equals(f.Name, valueString, StringComparison.Ordinal));
            if (enumField is not null)
            {
                var enumMemberValue = enumField.GetCustomAttributes(typeof(EnumMemberAttribute), inherit: true)
                    .Cast<EnumMemberAttribute>()
                    .FirstOrDefault()?
                    .Value;
                if (enumMemberValue is not null)
                    return AddParameterInternal(url, parameterName, enumMemberValue);
            }
        }

        return AddParameterInternal(url, parameterName, valueString);
    }

    public static string AddParameter(string url, string parameterName, int? value)
    {
        return !value.HasValue ? url : AddParameterInternal(url, parameterName, value?.ToString(CultureInfo.InvariantCulture));
    }

    public static string AddParameter(string url, string parameterName, long? value)
    {
        return !value.HasValue ? url : AddParameterInternal(url, parameterName, value?.ToString(CultureInfo.InvariantCulture));
    }

    public static string AddParameter(string url, string parameterName, DateTime? date)
    {
        return !date.HasValue ? url : AddParameterInternal(url, parameterName, date.Value.ToString("O"));
    }

    public static string AddParameter(string url, string parameterName, long[] values)
    {
        return values is null ? url : AddParameterInternal(url, parameterName, string.Join(",", values.Select(v => v.ToString(CultureInfo.InvariantCulture))));
    }

    public static string AddArrayParameter(string url, string parameterName, string[] values)
    {
        if (values is null)
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

    public static string AddPageParams(string url, int? page, int? perPage)
    {
        if (page is not null)
        {
            url = AddParameter(url, "page", page.Value);
        }

        if (perPage is not null)
        {
            url = AddParameter(url, "per_page", perPage.Value);
        }

        return url;
    }

    private static string AddParameterInternal(string url, string parameterName, string stringValue)
    {
        var @operator = !url.Contains("?") ? "?" : "&";
        var formattedValue = WebUtility.UrlEncode(stringValue);
        var parameter = $"{@operator}{parameterName}={formattedValue}";
        return url + parameter;
    }
}
