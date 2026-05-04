using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartFix.Application.Features.PromoCodes.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.ValueObjects;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Application.Features.PromoCodes.Queries.GetAllPromoCodes;

public class GetAllPromoCodesQueryHandler : IRequestHandler<GetAllPromoCodesQuery, List<PromoCodeDto>>
{
    private readonly IPromoCodeRepository _promoCodeRepository;

    public GetAllPromoCodesQueryHandler(IPromoCodeRepository promoCodeRepository)
    {
        _promoCodeRepository = promoCodeRepository;
    }

    public async Task<List<PromoCodeDto>> Handle(GetAllPromoCodesQuery request, CancellationToken cancellationToken)
    {
        var promoCodes = await _promoCodeRepository.GetAllAsync(cancellationToken);
        return promoCodes.Select(p => new PromoCodeDto
            {
                Id = p.Id,
                Code = p.Code,
                ExpirationDate = p.ExpirationDate,
                IsActive = p.IsActive,
                IsValid = p.IsValid()
            })
            .OrderBy(p => p.ExpirationDate).ToList();
    }
}