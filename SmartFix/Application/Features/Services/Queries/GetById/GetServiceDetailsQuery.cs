using MediatR;
using SmartFix.Application.Features.Services.DTO;

namespace SmartFix.Application.Features.Services.Queries;

public class GetServiceDetailsQuery: IRequest<ServiceDetailsDto>
{
    public Guid ServiceId { get; set; }
}