using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.ServiceCategories.Commands.UpdateCategory;

public class UpdateServiceCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IServiceCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceCategoryCommandHandler(IServiceCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null) 
            throw new Exception($"Категория с ID {request.Id} не найдена.");

        category.UpdateName(request.Name);
        
        _categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}