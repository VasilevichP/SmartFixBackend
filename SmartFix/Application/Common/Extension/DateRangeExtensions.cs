using System.Net;
using SmartFix.Application.Common.Interfaces;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Common.Extension;

public static class DateRangeExtensions
{
    public static (DateTime Start, DateTime End) CalculateDateRange(this IDateRangeRequest request)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate;

        if (request.Period == "custom")
        {
            startDate = request.From.HasValue ? request.From.Value.Date : endDate.AddDays(-30);
            if (request.To.HasValue)
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
        if ((endDate - startDate).TotalDays > 366)
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Период отчета не может превышать 1 год. Пожалуйста, уменьшите диапазон дат.");
        }


        return (startDate, endDate);
    }
}