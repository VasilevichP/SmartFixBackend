using MediatR;

namespace SmartFix.Application.Features.ServiceCategories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest
{
    public string Name { get; set; }
}