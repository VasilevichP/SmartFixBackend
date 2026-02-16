using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.AssignPrice;

public class AssignPriceCommandHandler: IRequestHandler<AssignPriceCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignPriceCommandHandler(
        IRequestRepository requestRepository,
        IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AssignPriceCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (requestEntity == null) throw new HttpException(HttpStatusCode.NotFound,"Заявка не найдена");
        
        requestEntity.AssignPrice(request.Price);

        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}