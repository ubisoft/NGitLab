using System;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using NGitLab.Models;
using static NGitLab.Impl.Json.ReflectionExtensions;

namespace NGitLab.Impl
{
    internal static class QueryStringHelper
    {
        public static string BuildAndAppendQueryString<T>(string url, T query)
        {
            var queryString = BuildQueryString(query);
            return string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
        }

        public static string BuildQueryString<T>(T query)
        {
            var sb = new StringBuilder();
            var type = typeof(T);

            foreach (var memberInfo in type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (memberInfo.MemberType != MemberTypes.Field &&
                    memberInfo.MemberType != MemberTypes.Property)
                {
                    continue;
                }

                if (memberInfo.GetCustomAttribute(typeof(QueryParameterAttribute), inherit: true) is not QueryParameterAttribute queryParameter)
                    continue;

                var value = memberInfo.MemberType switch
                {
                    MemberTypes.Field => ((FieldInfo)memberInfo).GetValue(query),
                    MemberTypes.Property => ((PropertyInfo)memberInfo).GetValue(query),
                    _ => throw new InvalidOperationException("This code should never execute"),
                };

                if (value is null)
                    continue;

                var strValue = FormatValue(value);
                if (string.IsNullOrEmpty(strValue))
                    continue;

                if (sb.Length > 0)
                    sb.Append('&');

                sb.Append(queryParameter.Name)
                  .Append('=')
                  .Append(WebUtility.UrlEncode(strValue));
            }

            return sb.ToString();
        }

        private static string FormatValue(object value)
        {
            return value switch
            {
                string s => s,
                bool b => b.ToString(),
                int i => FormatIntValue(i),
                DateTime dt => FormatDateTimeValue(dt),
                DateTimeOffset dto => FormatDateTimeOffsetValue(dto),
                Enum e => FormatEnumValue(e),
                _ => /*value.ToString()*/throw new InvalidOperationException($"Could not format '{value}' as string)"),
            };
        }

        private static string FormatIntValue(int i) => i.ToString(CultureInfo.InvariantCulture);

        private static string FormatDateTimeOffsetValue(DateTimeOffset dto) => FormatDateTimeValue(dto.UtcDateTime);

        private static string FormatDateTimeValue(DateTime dt) => dt.ToString("O");

        private static string FormatEnumValue(Enum e)
        {
            var mappings = e.GetType().GetEnumMappings();
            return /*mappings.FirstOrDefault(em => em.EnumValue.Equals(e)).StringValue ??*/ e.ToString();
        }
    }
}
