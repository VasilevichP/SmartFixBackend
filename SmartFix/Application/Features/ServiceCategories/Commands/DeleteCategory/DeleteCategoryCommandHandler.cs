using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.ServiceCategories.Commands.DeleteCategory;

public class DeleteServiceCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IServiceCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceCategoryCommandHandler(IServiceCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null) return;

        // TODO: ВАЖНО! Перед удалением категории нужно проверить, не привязаны ли к ней какие-либо услуги.
        // if (category.Services.Any()) 
        // {
        //     throw new Exception("Невозможно удалить категорию, так как к ней привязаны услуги.");
        // }
        // Для этого нужно будет в GetByIdAsync добавить .Include(c => c.Services)

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}