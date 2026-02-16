using MediatR;

namespace SmartFix.Application.Common.Events;

public record RequestCreatedEvent(Guid RequestId, string Email, string ClientName) : INotification;