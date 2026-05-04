using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartFix.Application.Features.PromoCodes.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Application.Features.PromoCodes.Queries.GetPromoCodeById;

public class GetPromoCodeByIdQueryHandler: IRequestHandler<GetPromoCodeByIdQuery, PromoCodeDetailsDto>
{
    private readonly IPromoCodeRepository _promoCodeRepository;

    public GetPromoCodeByIdQueryHandler(IPromoCodeRepository promoCodeRepository)
    {
        _promoCodeRepository = promoCodeRepository;
    }

    public async Task<PromoCodeDetailsDto> Handle(GetPromoCodeByIdQuery request, CancellationToken cancellationToken)
    {
        var promoCode = await _promoCodeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (promoCode == null)
            throw new HttpException(HttpStatusCode.NotFound, "Скидка не найдена.");

        return new PromoCodeDetailsDto
        {
            Id = promoCode.Id,
            Code = promoCode.Code,
            Type = promoCode.Type,
            Value = promoCode.Value,
            ExpirationDate = promoCode.ExpirationDate,
            UsageLimit = promoCode.UsageLimit,
            IsActive = promoCode.IsActive,
            IsValid = promoCode.IsValid()
        };
    }
}