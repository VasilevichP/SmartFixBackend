using System.Net;
using MediatR;
using SmartFix.Application.Features.Requests.Commands.ChangeDeviceInfo;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.ChangeContactInfo;

public class ChangeContactInfoCommandHandler : IRequestHandler<ChangeContactInfoCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeContactInfoCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeContactInfoCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (requestEntity == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена.");
        }

        requestEntity.UpdateContactInfo(request.ContactName, request.ContactEmail, request.ContactPhone);

        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}