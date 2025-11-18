using MediatR;

namespace SmartFix.Application.Features.ServiceCategories.Commands.DeleteCategory;

public class DeleteCategoryCommand:IRequest
{
    public Guid Id { get; set; }
}