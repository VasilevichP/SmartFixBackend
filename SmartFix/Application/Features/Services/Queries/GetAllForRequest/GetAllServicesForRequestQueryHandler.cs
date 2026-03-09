using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Queries.GetAllForRequest;

public class GetAllServicesForRequestQueryHandler:IRequestHandler<GetAllServicesForRequestQuery, List<ServiceForRequestDto>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllServicesForRequestQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<List<ServiceForRequestDto>> Handle(GetAllServicesForRequestQuery request, CancellationToken cancellationToken)
    {
        return await _serviceRepository.GetAllForRequestAsync(cancellationToken);
    }
}