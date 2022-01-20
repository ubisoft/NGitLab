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
            return DateTime.ParseExact(
                str,
                GitLabTimeSettings.Iso8601Formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var str = value.ToUniversalTime().ToString(
                GitLabTimeSettings.Iso8601Formats[0],
                CultureInfo.InvariantCulture);
            writer.WriteStringValue(str);
        }
    }

    internal sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return DateTimeOffset.MinValue;

            var str = reader.GetString();
            return DateTimeOffset.ParseExact(
                str,
                GitLabTimeSettings.Iso8601Formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            var str = value.ToUniversalTime().ToString(
                GitLabTimeSettings.Iso8601Formats[0],
                CultureInfo.InvariantCulture);
            writer.WriteStringValue(str);
        }
    }

    internal static class GitLabTimeSettings
    {
        public static readonly string[] Iso8601Formats = new string[]
        {
            @"yyyy-MM-dd\THH:mm:ss.FFFFFFFK",
            @"yyyy-MM-dd\THH:mm:ss.FFFFFFF\Z",
            @"yyyy-MM-dd\THH:mm:ss\Z",
            @"yyyy-MM-dd\THH:mm:ssK",
            @"yyyy-MM-ddK",
            @"yyyy-MM-dd\Z",
        };
    }
}
