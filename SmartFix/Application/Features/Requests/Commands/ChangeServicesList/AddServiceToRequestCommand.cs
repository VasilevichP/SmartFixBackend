using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.ChangeServicesList;

public class AddServiceToRequestCommand:IRequest
{
    public Guid RequestId { get; set; }
    public Guid? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public decimal? ServicePrice { get; set; }
}