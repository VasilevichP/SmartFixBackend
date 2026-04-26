using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.PromoCodes.Commands.ChangePromoCodeStatus;

public class ChangePromoCodeStatusCommandHandler : IRequestHandler<ChangePromoCodeStatusCommand>
{
    private readonly IPromoCodeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePromoCodeStatusCommandHandler(IPromoCodeRepository repo, IUnitOfWork uow)
    {
        _repository = repo;
        _unitOfWork = uow;
    }

    public async Task Handle(ChangePromoCodeStatusCommand request, CancellationToken ct)
    {
        var promo = await _repository.GetByIdAsync(request.Id, ct);
        if (promo == null) throw new HttpException(HttpStatusCode.NotFound, "Промокод не найден.");

        promo.ChangeStatus();

        _repository.Update(promo);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}