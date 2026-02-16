using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.DTO;

public class StatusHistoryDto
{
    public RequestStatus Status { get; set; }
    public DateTime Date { get; set; }
}