using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Discounts.Commands.ChangeDiscountStatus;

public class ChangeDiscountStatusCommandHandler:IRequestHandler<ChangeDiscountStatusCommand>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeDiscountStatusCommandHandler(IDiscountRepository discountRepository, IUnitOfWork unitOfWork)
    {
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(ChangeDiscountStatusCommand request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (discount == null)
            throw new HttpException(HttpStatusCode.NotFound, "Скидка не найдена.");
        
        discount.ChangeStatus();
        _discountRepository.Update(discount);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}