using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.EventHandlers;

public class RequestServicesChangedEventHandler: INotificationHandler<RequestServicesChangedEvent>
{
    private readonly IEmailService _emailService;

    public RequestServicesChangedEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(RequestServicesChangedEvent notification, CancellationToken cancellationToken)
    {
        var subject = $"Согласование заявки";

        var body = $@"
            <h2>Здравствуйте, {notification.ClientName}!</h2>
            <p>Список работ Вашей заявки изменен и требует согласования</p>
            <p><strong>Номер заявки:</strong> {notification.RequestId}</p>
            <p>Для согласования перейдите в личный кабинет или свяжитесь с нашим менеджером</p>
            <br>
            <p><em>С уважением, команда SmartFix</em></p>
        ";

        await _emailService.SendEmailAsync(notification.Email, subject, body, cancellationToken);
    }
}