using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForManager;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Requests.Queries.GetAllRequestsForMaster;

public class GetAllRequestsForMasterQueryHandler: IRequestHandler<GetAllRequestsForMasterQuery, List<RequestsBriefDto>>
{
    private readonly IRequestRepository _requestRepository;

    public GetAllRequestsForMasterQueryHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<List<RequestsBriefDto>> Handle(GetAllRequestsForMasterQuery request, CancellationToken cancellationToken)
    {
        var requests = await _requestRepository.GetAllForMasterAsync(
            request.MasterId,
            request.Client?.ToLower(),
            request.Device?.ToLower(),
            request.Status,
            request.SortOrder,
            cancellationToken);

        return requests.Select(r => new RequestsBriefDto
        {
            Id = r.Id,
            ClientName = r.ContactName,
            DeviceModelName = r.DeviceModelName,
            CreatedAt = r.CreatedAt,
            Status = r.Status,
            SpecialistName = r.Master?.Name ?? "Не назначен"
        }).ToList();
    }
}