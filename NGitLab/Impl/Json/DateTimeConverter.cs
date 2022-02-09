using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NGitLab.Impl.Json
{
    internal sealed class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return DateTime.MinValue;

            var str = reader.GetString();
            return str.TryParseAsDateOnly(out DateTime dateOnly) ? dateOnly : str.ParseIso8601DateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToIso8601String());
        }
    }

    internal sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return DateTimeOffset.MinValue;

            var str = reader.GetString();
            return str.TryParseAsDateOnly(out DateTimeOffset dateOnly) ? dateOnly : str.ParseIso8601DateTimeOffset();
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToIso8601String());
        }
    }

    internal static class DateTimeFormatter
    {
        private const string DateOnlyFormat = "yyyy-MM-dd";
        private const string Iso8601Format = "yyyy-MM-ddTHH:mm:ss.FFFFFFFK";

        public static bool TryParseAsDateOnly(this string str, out DateTime dateOnly)
        {
            return DateTime.TryParseExact(
                str,
                DateOnlyFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateOnly);
        }

        public static bool TryParseAsDateOnly(this string str, out DateTimeOffset dateOnly)
        {
            return DateTimeOffset.TryParseExact(
                str,
                DateOnlyFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateOnly);
        }

        public static DateTime ParseIso8601DateTime(this string str)
        {
            return DateTime.ParseExact(
                str,
                Iso8601Format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public static DateTimeOffset ParseIso8601DateTimeOffset(this string str)
        {
            return DateTimeOffset.ParseExact(
                str,
                Iso8601Format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public static string ToIso8601String(this DateTime value)
        {
            var str = value.ToUniversalTime().ToString(
                Iso8601Format,
                CultureInfo.InvariantCulture);
            return str;
        }

        public static string ToIso8601String(this DateTimeOffset value)
        {
            var str = value.ToUniversalTime().ToString(
                Iso8601Format,
                CultureInfo.InvariantCulture);
            return str;
        }
    }
}
