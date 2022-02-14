using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NGitLab.Impl.Json
{
    internal sealed class Int32Converter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return 0;

            if (reader.TokenType == JsonTokenType.String)
            {
                if (int.TryParse(reader.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
                    return i;

                // pipeline.coverage is a float, but the model is a int...
                if (double.TryParse(reader.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    return (int)d;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                // commitstatus.coverage is a float, but the model is a int...
                if (reader.ValueSpan.IndexOf((byte)'.') > 0)
                {
                    return (int)reader.GetDouble();
                }
            }

            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
