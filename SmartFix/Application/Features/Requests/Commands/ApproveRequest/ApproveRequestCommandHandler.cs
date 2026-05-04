using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.ApproveRequest;

public class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveRequestCommandHandler(IRequestRepository repo, IUnitOfWork uow)
    {
        _requestRepository = repo;
        _unitOfWork = uow;
    }

    public async Task Handle(ApproveRequestCommand request, CancellationToken ct)
    {
        var entity = await _requestRepository.GetByIdAsync(request.Id, ct);
        if (entity == null) throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена");

        entity.ChangeStatus(RequestStatus.InProgress);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}