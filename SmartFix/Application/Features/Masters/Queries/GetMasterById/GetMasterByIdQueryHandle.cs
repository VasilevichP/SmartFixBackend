using System.Net;
using MediatR;
using SmartFix.Application.Features.Clients.DTO;
using SmartFix.Application.Features.Masters.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Masters.Queries.GetMasterById;

public class GetMasterByIdQueryHandle:IRequestHandler<GetMasterByIdQuery, MasterDetailsDto>
{
    private readonly IUserRepository _userRepository;

    public GetMasterByIdQueryHandle(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<MasterDetailsDto> Handle(GetMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var master = await _userRepository.GetMasterByIdAsync(request.Id, cancellationToken);
        if (master == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        }
        
        var activeRequestsCount = await _userRepository.CountMasterActiveRequestsAsync(master.Id, cancellationToken);

        return new MasterDetailsDto()
        {
            Id = master.Id,
            Email = master.Email,
            Name = master.Name,
            PhoneNumber = master.PhoneNumber,
            ActiveRequestsCount = activeRequestsCount
        };
    }
}