using System.Net;
using MediatR;
using SmartFix.Application.Features.Services.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Services.Queries.GetById;

public class GetServiceDetailsQueryHandler : IRequestHandler<GetServiceDetailsQuery, ServiceDetailsDto>
{
    private readonly IServiceRepository _serviceRepository;

    public GetServiceDetailsQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ServiceDetailsDto> Handle(GetServiceDetailsQuery request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId, cancellationToken);

        if (service == null)
            throw new HttpException(HttpStatusCode.NotFound, "Услуга не найдена");

        return new ServiceDetailsDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price,
            WarrantyPeriod = service.WarrantyPeriod,
            CategoryId = service.CategoryId,
            CategoryName = service.Category.Name,
            DeviceTypeId = service.DeviceTypeId,
            DeviceTypeName = service.DeviceType.Name,
            DeviceModelId = service.DeviceModelId,
            DeviceModelName = service.DeviceModel?.Name,
            ManufacturerId = service.ManufacturerId,
            ManufacturerName = service.Manufacturer?.Name,
            IsAvailable = service.IsAvailable,
        };
    }
}