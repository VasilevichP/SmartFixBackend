using MediatR;
using SmartFix.Application.Features.ServiceCategories.DTO;

namespace SmartFix.Application.Features.ServiceCategories.Queries.GetAll;

public class GetAllQuery : IRequest<List<ServiceCategoryDto>>
{
}