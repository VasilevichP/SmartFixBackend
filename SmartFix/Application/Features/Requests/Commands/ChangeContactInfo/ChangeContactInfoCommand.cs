using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.ChangeContactInfo;

public class ChangeContactInfoCommand : IRequest
{
    public Guid Id { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
}