namespace SmartFix.Application.Features.Specialists.DTO;

public class SpecialistWithLoadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int ActiveRequestsCount { get; set; }
}