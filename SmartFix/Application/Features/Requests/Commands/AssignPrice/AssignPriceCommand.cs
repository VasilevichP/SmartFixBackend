using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.AssignPrice;

public class AssignPriceCommand: IRequest
{
    public Guid RequestId { get; set; }
    public decimal Price { get; set; }
}