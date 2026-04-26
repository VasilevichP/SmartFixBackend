using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.Commands.CreateDiscount;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand>
{
    private readonly IDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscountCommandHandler(IDiscountRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateDiscountCommand cmd, CancellationToken ct)
    {
        try
        {
            Discount rule = cmd.Category switch
            {
                DiscountCategory.ByCount => RequestsCountDiscount.Create(cmd.Name, int.Parse(cmd.ConditionValue),
                    cmd.Type,
                    cmd.Value, cmd.Priority),
                DiscountCategory.ByDay => DayOfWeekDiscount.Create(cmd.Name, Enum.Parse<DayOfWeek>(cmd.ConditionValue),
                    cmd.Type, cmd.Value, cmd.Priority),
                DiscountCategory.BySum => RequestSumDiscount.Create(cmd.Name, decimal.Parse(cmd.ConditionValue),
                    cmd.Type,
                    cmd.Value, cmd.Priority),
                _ => throw new Exception("Неверная категория скидки")
            };

            await _repository.AddAsync(rule, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (FormatException e)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Возникла ошибка при преобразовании условия скидки");
        }
    }
}