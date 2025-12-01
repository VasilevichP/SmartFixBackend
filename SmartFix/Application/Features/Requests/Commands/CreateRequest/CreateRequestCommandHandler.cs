using MediatR;
using SmartFix.Application.Abstractions;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public CreateRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork,
        IFileService fileService)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var domainRequest = Request.Create(
            clientId: request.ClientId,
            serviceId: request.ServiceId,
            deviceTypeId: request.DeviceTypeId,
            deviceModel: request.DeviceModel,
            description: request.Description,
            deviceSerialNumber: request.DeviceSerialNumber
        );
        if (request.Photos != null && request.Photos.Count > 0)
        {
            if (request.Photos.Count > 5)
                throw new Exception("Максимальное количество фото — 5.");

            foreach (var file in request.Photos)
            {
                var savedPath = await _fileService.SaveFileAsync(file, "requests", cancellationToken);
                domainRequest.AddPhoto(file.FileName, savedPath);
            }
        }

        await _requestRepository.AddAsync(domainRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return domainRequest.Id;
    }
}