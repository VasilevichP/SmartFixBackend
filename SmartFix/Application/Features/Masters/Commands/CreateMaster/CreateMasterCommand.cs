using MediatR;

namespace SmartFix.Application.Features.Masters.Commands.CreateMaster;

public class CreateMasterCommand:IRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}