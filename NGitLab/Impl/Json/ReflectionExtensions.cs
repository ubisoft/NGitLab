using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace NGitLab.Impl.Json
{
    internal static class ReflectionExtensions
    {
        public static IEnumerable<EnumMapping> GetEnumMappings(this Type type)
        {
            if (!type.IsEnum)
                yield break;

            foreach (var field in type.GetFields().Where(fi => fi.FieldType == type))
            {
                var enumValue = field.GetValue(null);

                var stringValue = field.GetCustomAttributes(typeof(EnumMemberAttribute), inherit: true)
                    .Cast<EnumMemberAttribute>()
                    .FirstOrDefault()?
                    .Value;

                yield return new EnumMapping((Enum)enumValue, stringValue);
            }
        }
    }

    internal readonly struct EnumMapping
    {
        public Enum EnumValue { get; }

        public string StringValue { get; }

        public EnumMapping(Enum enumValue, string stringValue)
        {
            EnumValue = enumValue;
            StringValue = stringValue;
        }

        public override readonly bool Equals(object obj)
        {
            return obj is EnumMapping other &&
                   EqualityComparer<Enum>.Default.Equals(EnumValue, other.EnumValue) &&
                   StringComparer.OrdinalIgnoreCase.Equals(StringValue, other.StringValue);
        }

        public override readonly int GetHashCode()
        {
            var hashCode = -1030903623;
            hashCode = (hashCode * -1521134295) + EqualityComparer<Enum>.Default.GetHashCode(EnumValue);
            hashCode = (hashCode * -1521134295) + StringComparer.OrdinalIgnoreCase.GetHashCode(StringValue);
            return hashCode;
        }
    }
}
