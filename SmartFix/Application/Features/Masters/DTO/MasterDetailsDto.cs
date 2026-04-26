namespace SmartFix.Application.Features.Masters.DTO;

public class MasterDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int ActiveRequestsCount { get; set; }
}