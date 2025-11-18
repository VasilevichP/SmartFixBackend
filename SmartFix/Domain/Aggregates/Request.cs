using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class Request
{
    public Guid Id { get; private set; }
    public RequestStatus Status { get; private set; }
    public string Description { get; private set; }
    public string? DeviceSerialNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }

    public Guid ClientId { get; private set; }
    public User Client { get; private set; }
    
    public Guid ServiceId { get; private set; }
    public Service Service { get; private set; }
    
    private readonly List<StatusHistory> _statusHistories = new();
    public IReadOnlyCollection<StatusHistory> StatusHistories => _statusHistories.AsReadOnly();

    private Request() { }

    // Фабричный метод для создания новой заявки
    public static Request Create(Guid clientId, Guid serviceId, string description, string? deviceSerialNumber)
    {
        var request = new Request
        {
            Id = Guid.NewGuid(),
            Status = RequestStatus.New,
            ClientId = clientId,
            ServiceId = serviceId,
            Description = description,
            DeviceSerialNumber = deviceSerialNumber,
            CreatedAt = DateTime.UtcNow
        };
        
        request.AddStatusHistory(RequestStatus.New);
        
        return request;
    }
    
    public void ChangeStatus(RequestStatus newStatus)
    {
        if (Status == newStatus) return;

        Status = newStatus;
        AddStatusHistory(newStatus);

        if (newStatus == RequestStatus.Closed || newStatus == RequestStatus.Cancelled)
        {
            ClosedAt = DateTime.UtcNow;
        }
        else
        {
            ClosedAt = null;
        }
    }
    
    public void UpdateDetails(string description, string? deviceSerialNumber)
    {
        Description = description;
        DeviceSerialNumber = deviceSerialNumber;
    }
    
    private void AddStatusHistory(RequestStatus status)
    {
        _statusHistories.Add(StatusHistory.Create(this.Id, status));
    }
}