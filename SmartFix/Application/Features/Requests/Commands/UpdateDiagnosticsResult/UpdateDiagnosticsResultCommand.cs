using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.UpdateDiagnosticsResult;

public class UpdateDiagnosticsResultCommand : IRequest
{
    public Guid Id { get; set; }
    public string Result { get; set; }
}