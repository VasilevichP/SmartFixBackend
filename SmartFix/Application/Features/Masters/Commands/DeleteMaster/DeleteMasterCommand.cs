using MediatR;

namespace SmartFix.Application.Features.Masters.Commands.DeleteMaster;

public class DeleteMasterCommand:IRequest
{
    public Guid Id { get; set; }
}