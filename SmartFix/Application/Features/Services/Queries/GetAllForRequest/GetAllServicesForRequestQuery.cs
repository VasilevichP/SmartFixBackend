using MediatR;
using SmartFix.Application.Features.Requests.DTO;

namespace SmartFix.Application.Features.Services.Queries.GetAllForRequest;

public class GetAllServicesForRequestQuery:IRequest<List<ServiceForRequestDto>>
{
}