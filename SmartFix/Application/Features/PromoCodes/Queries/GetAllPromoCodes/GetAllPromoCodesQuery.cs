using MediatR;
using SmartFix.Application.Features.PromoCodes.DTO;

namespace SmartFix.Application.Features.PromoCodes.Queries.GetAllPromoCodes;

public class GetAllPromoCodesQuery : IRequest<List<PromoCodeDto>>
{
}