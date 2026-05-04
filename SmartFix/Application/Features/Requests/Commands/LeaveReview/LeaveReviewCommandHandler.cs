using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Application.Features.Requests.Commands.LeaveReview;

public class LeaveReviewCommandHandler : IRequestHandler<LeaveReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LeaveReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LeaveReviewCommand request, CancellationToken cancellationToken)
    {
        if (await _reviewRepository.ExistsByRequest(request.RequestId, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest,"Отзыв на эту заявку уже оставлен");

        var review = Review.Create(request.RequestId, request.ClientId, request.Rating, request.Comment);

        await _reviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}