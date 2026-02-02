using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Requests.Queries.GetRequestDetails;

public class GetRequestDetailsQueryHandler : IRequestHandler<GetRequestDetailsQuery, RequestDetailsDto>
{
    private readonly IRequestRepository _requestRepository;

    public GetRequestDetailsQueryHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<RequestDetailsDto> Handle(GetRequestDetailsQuery request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);

        if (requestEntity == null) throw new Exception("Заявка не найдена");

        return new RequestDetailsDto
        {
            Id = requestEntity.Id,
            Status = requestEntity.Status,
            
            DeviceType = requestEntity.DeviceType?.Name ?? "Неизвестно",
            DeviceModel = requestEntity.DeviceModelName,
            DeviceSerialNumber = requestEntity.DeviceSerialNumber,
            Description = requestEntity.Description,
            
            ServiceName = requestEntity.Service?.Name,
            Price = requestEntity.Service?.Price,
            WarrantyPeriod = requestEntity.Service.WarrantyPeriod,
            
            CreatedAt = requestEntity.CreatedAt,
            ClosedAt = requestEntity.ClosedAt,
            
            ClientName = requestEntity.Client.Name,
            SpecialistName = requestEntity.Specialist?.FullName,

            PhotoPaths = requestEntity.Photos.Select(p => p.FilePath).ToList(),

            History = requestEntity.StatusHistories
                .OrderByDescending(h => h.Timestamp)
                .Select(h => new StatusHistoryDto 
                { 
                    Status = h.Status.ToString(), 
                    Date = h.Timestamp 
                }).ToList()
        };
    }
}