using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.PromoCodes.Commands.UpdatePromoCode;

public class UpdatePromoCodeCommandHandler: IRequestHandler<UpdatePromoCodeCommand>
{
    private readonly IPromoCodeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePromoCodeCommandHandler(IPromoCodeRepository repo, IUnitOfWork uow) { _repository = repo; _unitOfWork = uow; }

    public async Task Handle(UpdatePromoCodeCommand request, CancellationToken ct)
    {
        var promo = await _repository.GetByIdAsync(request.Id, ct);
        if (promo == null) throw new HttpException(HttpStatusCode.NotFound,"Промокод не найден.");

        if (promo.Code != request.Code.ToUpper())
        {
            var existing = await _repository.GetByCodeAsync(request.Code, ct);
            if (existing != null) throw new HttpException(HttpStatusCode.BadRequest,"Такой код уже занят.");
        }

        promo.Update(request.Code, request.Type, request.Value, request.ExpirationDate, request.UsageLimit);

        _repository.Update(promo);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}