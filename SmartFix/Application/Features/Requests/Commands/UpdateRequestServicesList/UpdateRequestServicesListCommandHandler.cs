using System.Net;
using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.UpdateRequestServicesList;

public class UpdateRequestServicesListCommandHandler : IRequestHandler<UpdateRequestServicesListCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher; 

    public UpdateRequestServicesListCommandHandler(IRequestRepository reqRepo, IDiscountRepository discRepo,
        IUserRepository userRepo, IUnitOfWork uow, IPromoCodeRepository promoCodeRepository, IPublisher publisher)
    {
        _requestRepository = reqRepo;
        _discountRepository = discRepo;
        _userRepository = userRepo;
        _unitOfWork = uow;
        _promoCodeRepository = promoCodeRepository;
        _publisher = publisher;
    }

    public async Task Handle(UpdateRequestServicesListCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (requestEntity == null) throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена.");

        var mappedServices = request.Services.Select(s => (s.ServiceId, s.Name, s.Price,s.Warranty));

        requestEntity.ReplaceServices(mappedServices);

        var allActiveRules = await _discountRepository.GetAllAsync(cancellationToken);
        var activeOnly = allActiveRules.Where(d => d.IsActive).ToList();

        var client = await _userRepository.GetClientByIdAsync(requestEntity.ClientId, cancellationToken);
        var clientOrdersCount = 0;
        var clientPersonalDiscount = 0;
        if (client != null)
        {
            clientOrdersCount = await _userRepository.GetClientOrdersCountAsync(client.Id, cancellationToken);
            clientPersonalDiscount = client.PersonalDiscount;
        }
        PromoCode? appliedPromo = null;
        if (requestEntity.PromoCodeId.HasValue)
        {
            appliedPromo = await _promoCodeRepository.GetByIdAsync(requestEntity.PromoCodeId.Value, cancellationToken);
        }

        DiscountCalculator.CalculateAndApplyDiscounts(requestEntity, activeOnly, clientOrdersCount,
            clientPersonalDiscount, appliedPromo);
        
        await _publisher.Publish(new RequestServicesChangedEvent(
            requestEntity.Id,
            requestEntity.ContactEmail,
            requestEntity.ContactName
        ), cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}