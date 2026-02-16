using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.EventHandlers;

public class RequestCreatedEventHandler : INotificationHandler<RequestCreatedEvent>
{
    private readonly IEmailService _emailService;

    public RequestCreatedEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(RequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        var subject = $"Заявка #{notification.RequestId.ToString().Substring(0, 8)} принята";
        
        var body = $@"
            <h2>Здравствуйте, {notification.ClientName}!</h2>
            <p>Ваша заявка на ремонт успешно зарегистрирована.</p>
            <p><strong>Номер заявки:</strong> {notification.RequestId}</p>
            <p>В ближайшее время ожидайте звонок от нашего менеджера.</p>
            <br>
            <p><em>С уважением, команда SmartFix</em></p>
        ";

        await _emailService.SendEmailAsync(notification.Email, subject, body, cancellationToken);
    }
}