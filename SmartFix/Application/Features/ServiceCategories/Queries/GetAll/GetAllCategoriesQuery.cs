using MediatR;
using SmartFix.Application.Features.ServiceCategories.DTO;

namespace SmartFix.Application.Features.ServiceCategories.Queries.GetAll;

public class GetAllCategoriesQuery : IRequest<List<ServiceCategoryDto>>
{
}