using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.EventHandlers;

public class RequestCancelledEventHandler: INotificationHandler<RequestCancelledEvent>
{
    private readonly IEmailService _emailService;

    public RequestCancelledEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(RequestCancelledEvent notification, CancellationToken cancellationToken)
    {
        var subject = $"Заявка отменена";
        
        var body = $@"
            <h2>Здравствуйте, {notification.ClientName}!</h2>
            <p>Ваша заявка на ремонт была отменена.</p>
            <p><strong>Номер заявки:</strong> {notification.RequestId}</p>
            <br>
            <p><em>С уважением, команда SmartFix</em></p>
        ";

        await _emailService.SendEmailAsync(notification.Email, subject, body, cancellationToken);
    }
}