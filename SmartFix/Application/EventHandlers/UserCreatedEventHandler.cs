using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.EventHandlers;

public class UserCreatedEventHandler:INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailService _emailService;

    public UserCreatedEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var subject = $"Добро пожаловать!";
        
        var body = $@"
            <p>Вы были добавлены в систему</p>
            <br>
            <p>Ваш логин: {notification.Email}</p>
            <p>Ваш пароль: {notification.Password}</p>
            <br>
            <p><em>С уважением, команда SmartFix</em></p>
        ";

        await _emailService.SendEmailAsync(notification.Email, subject, body, cancellationToken);
    }
}