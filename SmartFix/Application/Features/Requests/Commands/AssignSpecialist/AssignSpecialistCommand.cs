using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.AssignSpecialist;

public class AssignSpecialistCommand: IRequest
    
{
    public Guid RequestId { get; set; }
    public Guid SpecialistId { get; set; }
}