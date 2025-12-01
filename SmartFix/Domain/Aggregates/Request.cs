using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class Request
{
    public Guid Id { get; private set; }
    public RequestStatus Status { get; private set; }
    public string Description { get; private set; }
    public Guid DeviceTypeId { get; private set; }
    public DeviceType DeviceType { get; private set; }
    public string DeviceModel { get; private set; }
    public string DeviceSerialNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public Guid? SpecialistId { get; private set; }
    public Specialist Specialist { get; private set; }
    public Guid ClientId { get; private set; }
    public User Client { get; private set; }
    public Guid ServiceId { get; private set; }
    public Service Service { get; private set; }
    private readonly List<RequestPhoto> _photos = new();
    public IReadOnlyCollection<RequestPhoto> Photos => _photos.AsReadOnly();
    private readonly List<StatusHistory> _statusHistories = new();
    public IReadOnlyCollection<StatusHistory> StatusHistories => _statusHistories.AsReadOnly();

    private Request()
    {
    }

    public static Request Create(Guid clientId, Guid serviceId, Guid deviceTypeId, string deviceModel,
        string description, string? deviceSerialNumber)
    {
        var request = new Request
        {
            Id = Guid.NewGuid(),
            Status = RequestStatus.New,
            ClientId = clientId,
            ServiceId = serviceId,
            DeviceTypeId = deviceTypeId,
            DeviceModel = deviceModel,
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

    public void AssignSpecialist(Guid specialistId)
    {
        SpecialistId = specialistId;
    }

    public void AddPhoto(string fileName, string filePath)
    {
        if (_photos.Count >= 5)
        {
            throw new InvalidOperationException("Нельзя добавить более 5 фотографий к одной заявке.");
        }

        _photos.Add(RequestPhoto.Create(this.Id, fileName, filePath));
    }
}