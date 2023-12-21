using System;

namespace NGitLab.Mock;

public static class StringExtensions
{
    public static bool Contains(this string source, string toCheck, StringComparison comparison)
    {
        return source?.IndexOf(toCheck, comparison) >= 0;
    }
}
