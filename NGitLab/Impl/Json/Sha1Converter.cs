using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NGitLab.Impl.Json
{
    internal sealed class Sha1Converter : JsonConverter<Sha1>
    {
        public override Sha1 Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException($"Was expecting start of '{type}' string value");
            var str = reader.GetString();
            return new Sha1(str);
        }

        public override void Write(Utf8JsonWriter writer, Sha1 value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
