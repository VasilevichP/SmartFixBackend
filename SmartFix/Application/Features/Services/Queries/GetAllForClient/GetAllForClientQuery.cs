using MediatR;
using SmartFix.Application.Features.Services.DTO;

namespace SmartFix.Application.Features.Services.Queries.GetAllForClient;

public class GetAllForClientQuery : IRequest<List<ServiceDTO>>
{
}