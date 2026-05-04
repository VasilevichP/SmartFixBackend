using System.Globalization;
using System.Net;
using MediatR;
using SmartFix.Application.Features.Discounts.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery, DiscountDetailsDto>
{
    private readonly IDiscountRepository _discountRepository;

    public GetDiscountByIdQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<DiscountDetailsDto> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id, cancellationToken);

        if (discount == null)
            throw new HttpException(HttpStatusCode.NotFound, "Скидка не найдена.");

        var dto = new DiscountDetailsDto
        {
            Id = discount.Id,
            Name = discount.Name,
            Type = discount.Type,
            Value = discount.Value,
            Priority = discount.Priority,
            IsActive = discount.IsActive
        };

        switch (discount)
        {
            case RequestsCountDiscount countDiscount:
                dto.Category = DiscountCategory.ByCount;
                dto.ConditionValue = countDiscount.TargetOrdersCount.ToString();
                break;

            case RequestSumDiscount sumDiscount:
                dto.Category = DiscountCategory.BySum;
                dto.ConditionValue = sumDiscount.TargetSum.ToString("0.00", CultureInfo.InvariantCulture);
                break;

            case DayOfWeekDiscount dayDiscount:
                dto.Category = DiscountCategory.ByDay;
                dto.ConditionValue = ((int)dayDiscount.TargetDayOfWeek).ToString();
                break;
        }

        return dto;
    }
}