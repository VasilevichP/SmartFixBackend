namespace SmartFix.Application.Features.Reviews.DTO;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string ClientName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}