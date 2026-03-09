using System.Net;
using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

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

        if (requestEntity == null) throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена");

        return new RequestDetailsDto
        {
            Id = requestEntity.Id,
            Status = requestEntity.Status,

            DeviceType = requestEntity.DeviceType?.Name ?? "Неизвестно",
            DeviceModel = requestEntity.DeviceModelName,
            DeviceSerialNumber = requestEntity.DeviceSerialNumber,
            Description = requestEntity.Description,

            Price = requestEntity.Price,
            Appearance = requestEntity.DeviceAppearance,
            Package = requestEntity.DevicePackage,
            
            DiagnosticResult = requestEntity.DiagnosticResult,
            CancellationReason = requestEntity.CancellationReason,

            CreatedAt = requestEntity.CreatedAt,
            ClosedAt = requestEntity.ClosedAt,

            ClientId = requestEntity.ClientId,
            ClientEmail = requestEntity.ContactEmail,
            ClientName = requestEntity.ContactName,
            ClientPhone = requestEntity.ContactPhoneNumber,
            IsCourierDelivery = requestEntity.IsCourierDelivery,
            Address = requestEntity.DeliveryAddress,
            DeliveryCost = requestEntity.DeliveryCost,

            SpecialistId = requestEntity.SpecialistId,
            SpecialistName = requestEntity.Specialist?.Name,
            
            Services = requestEntity.Services
                .Select(r=> new RequestServiceDto
                {
                    Id = r.Id,
                    ServiceId = r.ServiceId,
                    ServiceName = r.ServiceName,
                    Price = r.Price,
                }).ToList(),

            PhotoPaths = requestEntity.Photos.Select(p => p.FilePath).ToList(),

            History = requestEntity.StatusHistories
                .OrderByDescending(h => h.Timestamp)
                .Select(h => new StatusHistoryDto
                {
                    Status = h.Status,
                    Date = h.Timestamp
                }).ToList()
        };
    }
}