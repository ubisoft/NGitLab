using System;
using NGitLab.Extensions;

namespace NGitLab.Models;

public static class IdOrPathExtensions
{
    public static string ValueAsString(this IIdOrPathAddressable idOrPath)
        => idOrPath.Path ?? idOrPath.Id.ToStringInvariant();

    public static string ValueAsUriParameter(this IIdOrPathAddressable idOrPath)
        => idOrPath.Path is null ? idOrPath.Id.ToStringInvariant() : Uri.EscapeDataString(idOrPath.Path);
}
