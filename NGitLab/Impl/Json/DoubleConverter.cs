using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NGitLab.Impl.Json
{
    internal sealed class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return 0.0d;

            if (reader.TokenType == JsonTokenType.String)
            {
                if (double.TryParse(reader.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    return d;
            }

            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
