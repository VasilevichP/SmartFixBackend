using MediatR;
using SmartFix.Application.Features.Discounts.DTO;

namespace SmartFix.Application.Features.Discounts.Queries.GetAllDiscounts;

public class GetAllDiscountsQuery: IRequest<List<DiscountDto>> { }