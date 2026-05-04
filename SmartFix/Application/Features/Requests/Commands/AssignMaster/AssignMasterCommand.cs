using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.AssignMaster;

public class AssignMasterCommand: IRequest
    
{
    public Guid RequestId { get; set; }
    public Guid MasterId { get; set; }
}