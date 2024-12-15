using System;
using System.Collections.Generic;

namespace NGitLab;

/// <summary>
/// Allows to expose enums without knowing all the possible values
/// that can be serialized from the client.
/// </summary>
public readonly struct DynamicEnum<TEnum> : IEquatable<DynamicEnum<TEnum>>, IEquatable<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// This value is filled when the value is recognized.
    /// </summary>
    public TEnum? EnumValue { get; }

    /// <summary>
    /// Contains the serialized string when the value is not a known
    /// flag of the underlying enum.
    /// </summary>
    public string StringValue { get; }

    public DynamicEnum(TEnum enumValue)
    {
        EnumValue = enumValue;
        StringValue = null;
    }

    public DynamicEnum(string stringValue)
    {
        EnumValue = default;
        StringValue = stringValue;
    }

    public readonly bool Equals(TEnum other)
    {
        return Equals(EnumValue, other);
    }

    public readonly bool Equals(DynamicEnum<TEnum> other)
    {
        return EqualityComparer<TEnum?>.Default.Equals(EnumValue, other.EnumValue) &&
               StringComparer.OrdinalIgnoreCase.Equals(StringValue, other.StringValue);
    }

    public override readonly bool Equals(object obj)
    {
        return obj is DynamicEnum<TEnum> other && Equals(other);
    }

    public override readonly int GetHashCode()
    {
        return EqualityComparer<TEnum?>.Default.GetHashCode(EnumValue);
    }

    public static bool operator ==(DynamicEnum<TEnum> obj1, DynamicEnum<TEnum> obj2) => obj1.Equals(obj2);

    public static bool operator !=(DynamicEnum<TEnum> obj1, DynamicEnum<TEnum> obj2) => !obj1.Equals(obj2);

    public static bool operator ==(DynamicEnum<TEnum> obj1, TEnum obj2) => obj1.Equals(obj2);

    public static bool operator !=(DynamicEnum<TEnum> obj1, TEnum obj2) => !obj1.Equals(obj2);

    public override readonly string ToString()
    {
        return StringValue ?? EnumValue?.ToString() ?? string.Empty;
    }
}
