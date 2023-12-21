#nullable enable
using System.Globalization;

namespace NGitLab.Extensions;

internal static class NumberExtensions
{
    public static string ToStringInvariant(this byte number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this byte number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this sbyte number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this sbyte number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this short number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this short number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this ushort number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this ushort number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this int number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this int number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this uint number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this uint number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this long number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this long number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this ulong number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this ulong number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this float number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this float number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this double number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this double number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant(this decimal number)
    {
        return ToStringInvariant(number, format: null);
    }

    public static string ToStringInvariant(this decimal number, string? format)
    {
        if (format != null)
            return number.ToString(format, CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }
}
