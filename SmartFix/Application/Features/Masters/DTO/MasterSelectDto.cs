namespace SmartFix.Application.Features.Masters.DTO;

public class MasterSelectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ActiveRequestsCount { get; set; }
}