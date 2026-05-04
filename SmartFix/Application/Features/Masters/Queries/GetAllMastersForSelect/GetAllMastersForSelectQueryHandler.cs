using MediatR;
using SmartFix.Application.Features.Masters.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Masters.Queries.GetAllMastersForSelect;

public class GetAllMastersForSelectQueryHandler:IRequestHandler<GetAllMastersForSelectQuery, List<MasterSelectDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllMastersForSelectQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<MasterSelectDto>> Handle(GetAllMastersForSelectQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllMastersForSelect(cancellationToken);
    }
}