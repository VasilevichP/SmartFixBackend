using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.DTO;

public class RequestsBriefDto
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string SpecialistName { get; set; } = "Не назначен";
    public RequestStatus Status { get; set; }
    public string StatusName => Status.ToString();
}