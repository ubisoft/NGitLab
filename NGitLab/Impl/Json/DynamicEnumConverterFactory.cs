using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NGitLab.Models;

namespace NGitLab.Impl.Json
{
    internal sealed class DynamicEnumConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(DynamicEnum<>);
        }

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            var enumType = type.GenericTypeArguments[0];

            if (enumType == typeof(EventTargetType))
                return new DynamicEventTargetTypeConverter(options);

            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(DynamicEnumConverter<>).MakeGenericType(enumType),
                options);

            return converter;
        }

        private class DynamicEnumConverter<TEnum> : JsonConverter<DynamicEnum<TEnum>>
            where TEnum : struct, Enum
        {
            private readonly JsonConverter<TEnum> _enumConverter;
            private readonly Type _enumType;

            public DynamicEnumConverter(JsonSerializerOptions options)
            {
                // For performance, use the existing converter if available
                _enumConverter = (JsonConverter<TEnum>)options.GetConverter(typeof(TEnum));

                // Cache the enum type
                _enumType = typeof(TEnum);
            }

            public override DynamicEnum<TEnum> Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                    throw new JsonException($"Was expecting start of '{_enumType}' string value");

                var reader2 = reader;
                try
                {
                    // First, try to deserialize the underlying enum the standard way.
                    // This will work if we have a known value.
                    var enumValue =
                        _enumConverter?.Read(ref reader, _enumType, options) ??
                        JsonSerializer.Deserialize<TEnum>(ref reader, options);
                    return new DynamicEnum<TEnum>(enumValue);
                }
                catch
                {
                    var stringValue = reader2.GetString();
                    return new DynamicEnum<TEnum>(stringValue);
                }
            }

            public override void Write(Utf8JsonWriter writer, DynamicEnum<TEnum> dynamicEnum, JsonSerializerOptions options)
            {
                if (dynamicEnum.EnumValue.HasValue)
                {
                    var enumValue = dynamicEnum.EnumValue.Value;
                    if (_enumConverter != null)
                        _enumConverter.Write(writer, enumValue, options);
                    else
                        JsonSerializer.Serialize(writer, enumValue, options);
                }
                else
                {
                    writer.WriteStringValue(dynamicEnum.StringValue);
                }
            }
        }

        private sealed class DynamicEventTargetTypeConverter : DynamicEnumConverter<EventTargetType>
        {
            public DynamicEventTargetTypeConverter(JsonSerializerOptions options)
                : base(options)
            {
            }

            public override DynamicEnum<EventTargetType> Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                    return new DynamicEnum<EventTargetType>(EventTargetType.None);

                return base.Read(ref reader, type, options);
            }
        }
    }
}
