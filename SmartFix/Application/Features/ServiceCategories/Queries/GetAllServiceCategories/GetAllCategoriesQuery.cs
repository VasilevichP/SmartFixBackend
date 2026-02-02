using MediatR;
using SmartFix.Application.Features.ServiceCategories.DTO;

namespace SmartFix.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;

public class GetAllCategoriesQuery : IRequest<List<ServiceCategoryDto>>
{
}