using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.EventHandlers;

public class RequestReadyEventHandler : INotificationHandler<RequestReadyEvent>
{
    private readonly IEmailService _emailService;

    public RequestReadyEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(RequestReadyEvent notification, CancellationToken cancellationToken)
    {
        var subject = $"Заявка #{notification.RequestId.ToString().Substring(0, 8)} готова";

        var body = $@"
            <h2>Здравствуйте, {notification.ClientName}!</h2>
            <p>Ваше устройство отремонтировано и готово к выдаче.</p>
            <p><strong>Номер заявки:</strong> {notification.RequestId}</p>
            <p>В ближайшее время подойдите в сервис и заберите устройство.</p>
            <br>
            <p><em>С уважением, команда SmartFix</em></p>
        ";

        await _emailService.SendEmailAsync(notification.Email, subject, body, cancellationToken);
    }
}