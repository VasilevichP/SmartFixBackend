using SmartFix.Application.Common.Interfaces;

namespace SmartFix.Application.Common.Extension;

public static class DateRangeExtensions
{
    public static (DateTime Start, DateTime End) CalculateDateRange(this IDateRangeRequest request)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate;

        if (request.Period == "custom" && request.From.HasValue && request.To.HasValue)
        {
            startDate = request.From.Value.Date;
            endDate = request.To.Value.Date.AddDays(1).AddTicks(-1);
        }
        else
        {
            startDate = request.Period switch
            {
                "week" => endDate.AddDays(-7),
                "year" => endDate.AddYears(-1),
                _ => endDate.AddDays(-30) // "month" по умолчанию
            };
        }

        return (startDate, endDate);
    }
}