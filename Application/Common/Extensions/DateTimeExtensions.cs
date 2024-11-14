using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Extensions;

public static class DateTimeExtensions
{
    [return: NotNullIfNotNull(nameof(dateTime))]
    public static DateTime? SetKindUtc(this DateTime? dateTime)
    {
        if (dateTime is null)
        {
            return null;
        }

        return dateTime.Value.SetKindUtc();
    }

    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc) return dateTime;
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}
