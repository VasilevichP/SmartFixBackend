using MediatR;

namespace SmartFix.Application.Common.Events;

public record RequestReadyEvent(Guid RequestId, string Email, string ClientName) : INotification;