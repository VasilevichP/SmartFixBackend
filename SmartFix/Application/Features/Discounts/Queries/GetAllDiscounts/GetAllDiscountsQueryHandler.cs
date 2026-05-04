using MediatR;
using SmartFix.Application.Common.Extension;
using SmartFix.Application.Features.Discounts.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.Queries.GetAllDiscounts;

public class GetAllDiscountsQueryHandler : IRequestHandler<GetAllDiscountsQuery, List<DiscountDto>>
{
    private readonly IDiscountRepository _discountRepository;

    public GetAllDiscountsQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<List<DiscountDto>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
    {
        var discounts = await _discountRepository.GetAllAsync(cancellationToken);
        return discounts.Select(d =>
            {
                var dto = new DiscountDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Type = d.Type,
                    IsActive = d.IsActive
                };
                switch (d)
                {
                    case RequestsCountDiscount countDiscount:
                        dto.Category = DiscountCategory.ByCount;
                        break;

                    case RequestSumDiscount sumDiscount:
                        dto.Category = DiscountCategory.BySum;
                        break;

                    case DayOfWeekDiscount dayDiscount:
                        dto.Category = DiscountCategory.ByDay;
                        break;
                }

                return dto;
            })
            .ToList();
    }
}