using MediatR;

namespace SmartFix.Application.Common.Events;

public record RequestServicesChangedEvent(Guid RequestId, string Email, string ClientName) : INotification;