using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NGitLab.Models;

namespace NGitLab.Impl.Json
{
    internal sealed class EnumConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type type)
        {
            // AccessLevel will be serialized as int, using System.Text.Json's default enum serializer
            return type.IsEnum && type != typeof(AccessLevel);
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(EnumConverter<>).MakeGenericType(type));
            return converter;
        }

        private sealed class EnumConverter<TEnum> : JsonConverter<TEnum>
            where TEnum : struct, Enum
        {
            private readonly Type _enumType = typeof(TEnum);

            private readonly Dictionary<string, TEnum> _stringToEnumValues;
            private readonly Dictionary<TEnum, string> _enumToStringValues;

            public EnumConverter()
            {
                var enumValues = new List<TEnum>();
                var stringValues = new List<string>();

                foreach (var mapping in _enumType.GetEnumMappings())
                {
                    enumValues.Add((TEnum)mapping.EnumValue);
                    stringValues.Add(mapping.StringValue ?? mapping.EnumValue.ToString(CultureInfo.InvariantCulture));
                }

                _stringToEnumValues = new Dictionary<string, TEnum>(stringValues.Count, StringComparer.OrdinalIgnoreCase);
                _enumToStringValues = new Dictionary<TEnum, string>(stringValues.Count);

                for (var i = 0; i < stringValues.Count; i++)
                {
                    _stringToEnumValues[stringValues[i]] = enumValues[i];
                    _enumToStringValues[enumValues[i]] = stringValues[i];
                }
            }

            public override TEnum Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                    throw new JsonException($"Was expecting start of '{_enumType}' string value");

                var stringValue = reader.GetString();
                if (!_stringToEnumValues.TryGetValue(stringValue, out var enumValue))
                {
                    enumValue = (TEnum)Enum.Parse(_enumType, stringValue, ignoreCase: true);
                }

                return enumValue;
            }

            public override void Write(Utf8JsonWriter writer, TEnum enumValue, JsonSerializerOptions options)
            {
                if (!_enumToStringValues.TryGetValue(enumValue, out var stringValue))
                {
                    stringValue = enumValue.ToString();
                }

                writer.WriteStringValue(stringValue);
            }
        }
    }
}
