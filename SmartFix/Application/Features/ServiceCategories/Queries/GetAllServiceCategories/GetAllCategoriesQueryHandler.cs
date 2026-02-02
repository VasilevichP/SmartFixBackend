using MediatR;
using SmartFix.Application.Features.ServiceCategories.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<ServiceCategoryDto>>
{
    private readonly IServiceCategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(IServiceCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ServiceCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        
        return categories
            .Select(c => new ServiceCategoryDto { Id = c.Id, Name = c.Name })
            .ToList();
    }
}