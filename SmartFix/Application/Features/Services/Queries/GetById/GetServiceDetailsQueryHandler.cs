using MediatR;
using SmartFix.Application.Features.Reviews.DTO;
using SmartFix.Application.Features.Services.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Queries.GetById;

public class GetServiceDetailsQueryHandler: IRequestHandler<GetServiceDetailsQuery, ServiceDetailsDto>
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
            throw new Exception("Услуга не найдена");
        
        double avgRating = 0;
        if (service.Reviews.Any())
        {
            avgRating = service.Reviews.Average(r => r.Rating);
        }

        return new ServiceDetailsDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price,
            WarrantyPeriod = service.WarrantyPeriod,
            CategoryName = service.Category.Name,
            DeviceTypeName = service.DeviceType.Name,
            DeviceModelName = service.DeviceModel?.Name,
            ManufacturerName = service.Manufacturer?.Name,
            IsAvailable = service.IsAvailable,
            
            ReviewsCount = service.Reviews.Count,
            AverageRating = Math.Round(avgRating, 1), 
            
            Reviews = service.Reviews.OrderByDescending(r => r.CreatedAt).Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                ClientName = r.Client.Name
            }).ToList()
        };
    }
}