using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NGitLab.Impl.Json
{
    internal static class Serializer
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            // Change.AMode & BMode are integers, but deserialized from strings.
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            IncludeFields = true,
            Converters =
            {
                new Sha1Converter(),
                new EnumConverter(),
                new DynamicEnumConverter(),
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public static T Deserialize<T>(string json)
        {
            var obj = JsonSerializer.Deserialize<T>(json, _options);
            return obj ?? throw new InvalidOperationException($"Could not deserialize: {json}");
        }

        public static bool TryDeserializeObject(string json, out object obj)
        {
            try
            {
                obj = Deserialize<object>(json);
                return true;
            }
            catch (Exception /*ex*/)
            {
                obj = null;
                return false;
            }
        }

        public static string Serialize<T>(T obj)
        {
            var str = JsonSerializer.Serialize(obj, _options);
            return str;
        }
    }
}
