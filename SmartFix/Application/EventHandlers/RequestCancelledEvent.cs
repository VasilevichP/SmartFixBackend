using MediatR;

namespace SmartFix.Application.EventHandlers;

public record RequestCancelledEvent(Guid RequestId, string Email, string ClientName) : INotification;