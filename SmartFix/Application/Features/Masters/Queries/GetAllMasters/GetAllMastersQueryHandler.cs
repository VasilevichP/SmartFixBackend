using MediatR;
using SmartFix.Application.Features.Masters.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Masters.Queries.GetAllMasters;

public class GetAllMastersQueryHandler:IRequestHandler<GetAllMastersQuery, List<MasterDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllMastersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<MasterDto>> Handle(GetAllMastersQuery request, CancellationToken cancellationToken)
    {
        var masters = await _userRepository.GetMasterListAsync(request.nameSearch, request.phoneSearch, cancellationToken);
        return masters.Select(m => new MasterDto
            {
                Id = m.Id,
                Name = m.Name,
                PhoneNumber = m.PhoneNumber

            }
        ).ToList();
    }
}