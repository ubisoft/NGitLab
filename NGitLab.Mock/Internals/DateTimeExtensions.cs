using System;

namespace NGitLab.Mock;

internal static class DateTimeExtensions
{
    public static DateTimeOffset ToDateTimeOffsetAssumeUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Unspecified)
            return new DateTimeOffset(dateTime, TimeSpan.Zero);

        return new DateTimeOffset(dateTime);
    }
}
