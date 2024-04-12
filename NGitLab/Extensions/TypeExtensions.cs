using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace NGitLab.Extensions;

internal static class TypeExtensions
{
    public static string GetEnumMemberAttributeValue<TEnum>(this TEnum value)
        where TEnum : Enum
    {
        return typeof(TEnum)
            .GetTypeInfo()
            .DeclaredMembers
            .SingleOrDefault(x => string.Equals(x.Name, value.ToString(), StringComparison.Ordinal))
            ?.GetCustomAttribute<EnumMemberAttribute>(inherit: false)
            ?.Value;
    }
}
