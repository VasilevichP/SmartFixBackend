using MediatR;

namespace SmartFix.Application.Features.Masters.Commands.UpdateMaster;

public class UpdateMasterCommand:IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}