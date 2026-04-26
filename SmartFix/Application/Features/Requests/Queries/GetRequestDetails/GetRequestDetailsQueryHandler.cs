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
            Type = requestEntity.Type,
            Status = requestEntity.Status,
            CreatedAt = requestEntity.CreatedAt,
            ClosedAt = requestEntity.ClosedAt,
            
            ClientId = requestEntity.ClientId,
            ContactName = requestEntity.ContactName,
            ContactPhone = requestEntity.ContactPhoneNumber,
            ContactEmail = requestEntity.ContactEmail,
            
            DeviceTypeName = requestEntity.DeviceType?.Name ?? "Неизвестно",
            DeviceModelName = requestEntity.DeviceModelName,
            DeviceSerialNumber = requestEntity.DeviceSerialNumber,
            Description = requestEntity.Description,
            
            DiagnosticResult = requestEntity.DiagnosticResult,
            DeviceAppearance = requestEntity.DeviceAppearance,
            DevicePackage = requestEntity.DevicePackage,
            
            FieldAddress = requestEntity.FieldAddress,
            ScheduledTime = requestEntity.ScheduledTime,
            ParentRequestId = requestEntity.ParentRequestId,
            
            BasePrice = requestEntity.BasePrice,
            FinalPrice = requestEntity.FinalPrice,
            
            MasterId = requestEntity.MasterId,
            MasterName = requestEntity.Master?.Name,
           
            Services = requestEntity.Services.Select(s => new RequestServiceDto
            {
                Id = s.Id,
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                Price = s.Price
            }).ToList(),

            AppliedDiscounts = requestEntity.AppliedDiscounts.Select(d => new RequestDiscountDto
            {
                Id = d.Id,
                Name = d.RuleName,
                SavedAmount = d.SavedAmount
            }).ToList(),

            StatusHistories = requestEntity.StatusHistories
                .OrderBy(sh => sh.Timestamp)
                .Select(sh => new StatusHistoryDto
                {
                    Status = sh.Status,
                    Date = sh.Timestamp
                }).ToList(),
            PhotoPaths = requestEntity.Photos.Select(p => p.FilePath).ToList()
        };
    }
}