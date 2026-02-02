using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);
        if (review == null) throw new Exception("Отзыв не найден");

        if (request.UserRole != "Manager" && review.ClientId != request.UserId)
        {
            throw new Exception("Нет прав на удаление этого отзыва");
        }

        _reviewRepository.Delete(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}