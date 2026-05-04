namespace SmartFix.Application.Features.Requests.DTO;

public class RequestForSelectDto
{
    public Guid Id { get; set; }
    public string DeviceModelName { get; set; } = string.Empty;
    public DateTime? ClosedAt { get; set; }
}