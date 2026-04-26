using MediatR;

namespace SmartFix.Application.Common.Events;

public record UserCreatedEvent(string Email, string Password):INotification;