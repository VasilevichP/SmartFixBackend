using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.LeaveReview;

public class LeaveReviewCommand: IRequest
{
    public Guid RequestId { get; set; }
    public Guid ClientId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}