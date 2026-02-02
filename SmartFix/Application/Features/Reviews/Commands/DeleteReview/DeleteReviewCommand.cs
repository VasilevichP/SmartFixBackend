using MediatR;

namespace SmartFix.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommand: IRequest
{
    public Guid ReviewId { get; set; }
    public Guid UserId { get; set; }
    public string UserRole { get; set; }
}