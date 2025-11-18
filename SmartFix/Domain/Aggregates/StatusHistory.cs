using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class StatusHistory
{
    public Guid Id { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime Timestamp { get; private set; }
    public Guid RequestId { get; private set; }

    private StatusHistory() { }

    public static StatusHistory Create(Guid requestId, RequestStatus status)
    {
        return new StatusHistory
        {
            RequestId = requestId,
            Status = status,
            Timestamp = DateTime.UtcNow
        };
    }
}