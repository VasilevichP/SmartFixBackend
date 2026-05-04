using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.PromoCodes.Commands.CreatePromoCode;

public class CreatePromoCodeCommandHandler: IRequestHandler<CreatePromoCodeCommand>
{
    private readonly IPromoCodeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromoCodeCommandHandler(IPromoCodeRepository repo, IUnitOfWork uow) { _repository = repo; _unitOfWork = uow; }

    public async Task Handle(CreatePromoCodeCommand request, CancellationToken ct)
    {
        var existing = await _repository.GetByCodeAsync(request.Code, ct);
        if (existing != null) throw new HttpException(HttpStatusCode.BadRequest, "Такой промокод уже существует.");

        var promo = PromoCode.Create(request.Code, request.Type, request.Value, request.ExpirationDate,
            request.UsageLimit);

        await _repository.AddAsync(promo, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}