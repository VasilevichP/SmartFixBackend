using MediatR;
using SmartFix.Application.Features.PromoCodes.DTO;

namespace SmartFix.Application.Features.PromoCodes.Queries.GetPromoCodeById;

public class GetPromoCodeByIdQuery: IRequest<PromoCodeDetailsDto>
{
    public Guid Id { get; set; }
}