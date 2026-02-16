using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

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
        if (category == null) throw new HttpException(HttpStatusCode.NotFound, "Выбранная категория не найдена");

        if (await _categoryRepository.HasRelatedServicesAsync(category.Id, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя удалить Категорию: к ней привязаны услуги.");

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}