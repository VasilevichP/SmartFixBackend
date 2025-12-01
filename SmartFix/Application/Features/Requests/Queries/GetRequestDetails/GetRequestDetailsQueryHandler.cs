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
        var entity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);

        if (entity == null) throw new Exception("Заявка не найдена");

        return new RequestDetailsDto
        {
            Id = entity.Id,
            Status = entity.Status,
            
            DeviceType = entity.DeviceType?.Name ?? "Неизвестно",
            DeviceModel = entity.DeviceModel,
            DeviceSerialNumber = entity.DeviceSerialNumber,
            Description = entity.Description,
            
            ServiceName = entity.Service.Name,
            Price = entity.Service.Price,
            WarrantyPeriod = entity.Service.WarrantyPeriod,
            
            CreatedAt = entity.CreatedAt,
            ClosedAt = entity.ClosedAt,
            
            ClientName = $"{entity.Client.FirstName} {entity.Client.LastName}",
            SpecialistName = entity.Specialist?.FullName,

            PhotoPaths = entity.Photos.Select(p => p.FilePath).ToList(),

            History = entity.StatusHistories
                .OrderByDescending(h => h.Timestamp)
                .Select(h => new StatusHistoryDto 
                { 
                    Status = h.Status.ToString(), 
                    Date = h.Timestamp 
                }).ToList()
        };
    }
}