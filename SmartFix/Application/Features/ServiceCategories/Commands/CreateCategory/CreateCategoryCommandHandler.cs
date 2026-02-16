using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.ServiceCategories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
{
    private readonly IServiceCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IServiceCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.ExistsByName(request.Name, cancellationToken))
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Категория с таким названием уже существует");
        }
        var category = ServiceCategory.Create(request.Name);

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}