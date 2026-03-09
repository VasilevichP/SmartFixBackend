using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.UpdateAcceptanceInfo;

public class UpdateAcceptanceInfoCommand:IRequest
{
    public Guid Id { get; set; }
    public string Appearance { get; set; }
    public string Package { get; set; }
}