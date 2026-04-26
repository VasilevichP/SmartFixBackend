using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountCommandHandler:IRequestHandler<UpdateDiscountCommand>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository, IUnitOfWork unitOfWork)
    {
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (discount == null)
            throw new HttpException(HttpStatusCode.NotFound, "Скидка не найдена.");
       
        switch (discount)
        {
            case RequestsCountDiscount countDiscount:
                if (!int.TryParse(request.ConditionValue, out int targetOrders))
                    throw new HttpException(HttpStatusCode.BadRequest, "Для скидки по количеству заявок необходимо указать целое число.");
                countDiscount.Update(request.Name, targetOrders, request.Type, request.Value, request.Priority);
                break;

            case RequestSumDiscount sumDiscount:
                if (!decimal.TryParse(request.ConditionValue, out decimal targetSum))
                    throw new HttpException(HttpStatusCode.BadRequest, "Для скидки по сумме необходимо указать числовое значение.");
                sumDiscount.Update(request.Name, targetSum, request.Type, request.Value, request.Priority);
                break;

            case DayOfWeekDiscount dayDiscount:
                if (!Enum.TryParse(request.ConditionValue, out DayOfWeek targetDay))
                    throw new HttpException(HttpStatusCode.BadRequest, "Неверно указан день недели.");
                dayDiscount.Update(request.Name, targetDay, request.Type, request.Value, request.Priority);
                break;
                
            default:
                throw new HttpException(HttpStatusCode.BadRequest, "Неизвестный тип скидки.");
        }

        _discountRepository.Update(discount);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}