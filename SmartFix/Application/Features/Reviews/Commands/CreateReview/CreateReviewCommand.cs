using MediatR;

namespace SmartFix.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommand: IRequest
{
    public Guid ServiceId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public Guid ClientId { get; set; }
}