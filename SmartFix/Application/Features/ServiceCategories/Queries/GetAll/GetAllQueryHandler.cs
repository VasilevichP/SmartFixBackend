using MediatR;
using SmartFix.Application.Features.ServiceCategories.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.ServiceCategories.Queries.GetAll;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<ServiceCategoryDto>>
{
    private readonly IServiceCategoryRepository _categoryRepository;

    public GetAllQueryHandler(IServiceCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ServiceCategoryDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        
        return categories
            .Select(c => new ServiceCategoryDto { Id = c.Id, Name = c.Name })
            .ToList();
    }
}