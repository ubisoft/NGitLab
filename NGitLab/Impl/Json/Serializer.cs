using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NGitLab.Impl.Json;

internal static class Serializer
{
    private static readonly MyJsonSerializerContext _defaultSerializerContext = new(new()
    {
        IncludeFields = true,
        Converters =
        {
            new BooleanConverter(),
            new DateTimeConverter(),
            new DateTimeOffsetConverter(),
            new DoubleConverter(),
            new DynamicEnumConverterFactory(),
            new EnumConverterFactory(),
            new Int32Converter(),
            new Int64Converter(),
            new Sha1Converter(),
        },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    });

    public static T Deserialize<T>(string json)
    {
        var obj = (T)JsonSerializer.Deserialize(json, typeof(T), _defaultSerializerContext);
        return obj ?? throw new InvalidOperationException($"Could not deserialize: {json}");
    }

    public static T Deserialize<T>(ref Utf8JsonReader reader)
    {
        var obj = (T)JsonSerializer.Deserialize(ref reader, typeof(T), _defaultSerializerContext);
        return obj ?? throw new InvalidOperationException($"Could not deserialize");
    }

    public static bool TryDeserializeObject(string json, out object obj)
    {
        try
        {
            obj = Deserialize<object>(json);
            return true;
        }
        catch
        {
            obj = null;
            return false;
        }
    }

    public static string Serialize<T>(T obj)
    {
        var str = JsonSerializer.Serialize(obj, typeof(T), _defaultSerializerContext);
        return str;
    }

    public static string Serialize<T>(Utf8JsonWriter writer, T obj)
    {
        var str = JsonSerializer.Serialize(obj, typeof(T), _defaultSerializerContext);
        return str;
    }


}
