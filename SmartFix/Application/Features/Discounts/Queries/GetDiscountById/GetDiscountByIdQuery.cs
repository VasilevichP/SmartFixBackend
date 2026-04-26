using MediatR;
using SmartFix.Application.Features.Discounts.DTO;

namespace SmartFix.Application.Features.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdQuery:IRequest<DiscountDetailsDto>
{
    public Guid Id { get; set; }
}