using MediatR;
using SmartFix.Application.Features.Manufacturers.DTO;
using SmartFix.Application.Features.ServiceCategories.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Manufacturers.Queries.GetAllManufacturers;

public class GetAllManufacturersQueryHandler: IRequestHandler<GetAllManufacturersQuery, List<ManufacturerDto>>
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public GetAllManufacturersQueryHandler(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<List<ManufacturerDto>> Handle(GetAllManufacturersQuery request, CancellationToken cancellationToken)
    {
        var manufacturers = await _manufacturerRepository.GetAllAsync(cancellationToken);
        
        return manufacturers
            .Select(m => new ManufacturerDto { Id = m.Id, Name = m.Name })
            .ToList();
    }
}