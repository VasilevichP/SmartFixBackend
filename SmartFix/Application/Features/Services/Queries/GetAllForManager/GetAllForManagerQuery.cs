using MediatR;
using SmartFix.Application.Features.Services.DTO;

namespace SmartFix.Application.Features.Services.Queries.GetAllForManager;

public class GetAllForManagerQuery : IRequest<List<ServiceDTO>>
{
}