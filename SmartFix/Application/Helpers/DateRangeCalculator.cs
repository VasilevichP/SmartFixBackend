using System.Net;
using SmartFix.Application.Common.Interfaces;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Helpers;

public static class DateRangeCalculator
{
        public static (DateTime Start, DateTime End) CalculateDateRange(string period, DateTime? from, DateTime? to)
        {
            DateTime endDate = DateTime.UtcNow;
            DateTime startDate;

            if (period == "custom")
            {
                startDate = from.HasValue ? from.Value.Date : endDate.AddDays(-30);
                if (to.HasValue)
                    endDate = to.Value.Date.AddDays(1).AddTicks(-1);
            }
            else
            {
                startDate = period switch
                {
                    "week" => endDate.AddDays(-7),
                    "year" => endDate.AddYears(-1),
                    _ => endDate.AddDays(-30)
                };
            }
            if ((endDate - startDate).TotalDays > 366)
            {
                throw new HttpException(HttpStatusCode.BadRequest,"Период отчета не может превышать 1 год. Пожалуйста, уменьшите диапазон дат");
            }


            return (startDate, endDate);
        }
    
}