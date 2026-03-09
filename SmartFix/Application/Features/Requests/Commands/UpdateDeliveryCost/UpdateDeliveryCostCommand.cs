using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.UpdateDeliveryCost;

public class UpdateDeliveryCostCommand:IRequest
{
    public Guid Id { get; set; }
    public decimal DeliveryCost { get; set; }
}