using MediatR;

namespace SmartFix.Application.Features.ServiceCategories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}