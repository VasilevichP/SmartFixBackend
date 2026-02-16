namespace SmartFix.Application.Common.Interfaces;

public interface IDateRangeRequest
{
    string Period { get; }
    DateTime? From { get; }
    DateTime? To { get; }
}