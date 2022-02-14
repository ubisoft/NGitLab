using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NGitLab.Impl.Json
{
    internal sealed class Int64Converter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return 0;

            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt64();

            // Change.AMode & BMode are deserialized from strings...
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (long.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                    return value;
            }

            throw new JsonException("Unable to deserialize as long");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
