namespace SmartFix.Application.Features.Requests.DTO;

public class RequestDiscountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SavedAmount { get; set; }
}