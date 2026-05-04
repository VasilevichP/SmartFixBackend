using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.ChangeFieldRequestInfo;

public class ChangeFieldRequestInfoCommand:IRequest
{
    public Guid Id { get; set; }
    public string FieldAddress { get; set; } = string.Empty;
    public DateTime ScheduledTime { get; set; }
}