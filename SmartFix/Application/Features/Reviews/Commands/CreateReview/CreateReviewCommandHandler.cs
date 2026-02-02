using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler: IRequestHandler<CreateReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IReviewRepository reviewRepository, IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId, cancellationToken);
        if (service == null)
            throw new Exception("Услуга не найдена");

        var review = Review.Create(request.ServiceId, request.ClientId, request.Rating, request.Comment);

        await _reviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}