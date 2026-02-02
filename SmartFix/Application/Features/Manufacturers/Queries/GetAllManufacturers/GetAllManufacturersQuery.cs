using MediatR;
using SmartFix.Application.Features.Manufacturers.DTO;

namespace SmartFix.Application.Features.Manufacturers.Queries.GetAllManufacturers;

public class GetAllManufacturersQuery: IRequest<List<ManufacturerDto>>
{
}