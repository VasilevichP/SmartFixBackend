using System.Text;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Helpers;

public static class PdfDocsGenerator
{
    public static string GenerateCompletionActHtml(Request r)
    {
        var sb = new StringBuilder();

        sb.Append(@"
            <style>
                body { font-family: Arial, sans-serif; font-size: 14px; line-height: 1.4; color: #000; }
                .header-container { display: flex; justify-content: space-between; margin-bottom: 20px; }
                h1 { margin-bottom: 5px; font-size: 24px; }
                table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }
                th, td { border: 1px solid #000; padding: 8px; text-align: left; }
                th { background-color: #f2f2f2; font-weight: bold; }
                .bold { font-weight: bold; }
                .footer-text { font-size: 12px; margin-top: 20px; }
                .signatures { margin-top: 40px; }
            </style>");

        sb.Append($"<h1>Акт выполненных работ</h1>");
        sb.Append($"<p>Заказ № {r.Id.ToString().Substring(0, 8)} от {r.CreatedAt.ToLocalTime():dd.MM.yyyy}</p>");

        sb.Append("<table>");
        sb.Append($"<tr><td class='bold' width='30%'>Клиент</td><td>{r.ContactName}, {r.ContactPhoneNumber}</td></tr>");
        sb.Append($"<tr><td class='bold'>Устройство</td><td>{r.DeviceType?.Name}, {r.DeviceModelName}</td></tr>");
        sb.Append($"<tr><td class='bold'>Серийный номер</td><td>{r.DeviceSerialNumber}</td></tr>");
        sb.Append($"<tr><td class='bold'>Причина обращения</td><td>{r.Description}</td></tr>");
        sb.Append("</table>");

        sb.Append("<table>");
        sb.Append("<tr><th>№</th><th>Позиция</th><th>Гарантия</th><th>Цена</th><th>Количество</th><th>Сумма</th></tr>");

        int index = 1;
        decimal sum = 0;
        foreach (var service in r.Services)
        {
            string warrantyText = service.WarrantyPeriodMonths.HasValue
                ? $"{service.WarrantyPeriodMonths} мес."
                : "Без гарантии";
            sb.Append("<tr>");
            sb.Append($"<td>{index++}</td>");
            sb.Append($"<td>{service.ServiceName}</td>");
            sb.Append($"<td>{warrantyText}</td>");
            sb.Append($"<td>{service.Price:0.00} руб.</td>");
            sb.Append($"<td>1</td>");
            sb.Append($"<td>{service.Price:0.00} руб.</td>");
            sb.Append("</tr>");
            sum += service.Price;
        }

        sb.Append(
            $"<tr><td colspan='5' style='text-align: right;' class='bold'>Сумма (без учета скидок):</td><td class='bold'>{sum:0.00} руб.</td></tr>");

        foreach (var discount in r.AppliedDiscounts)
        {
            sb.Append(
                $"<tr><td colspan='5' style='text-align: right;'>Скидка ({discount.RuleName}):</td><td style='color: green;'>-{discount.SavedAmount:0.00} руб.</td></tr>");
        }

        sb.Append(
            $"<tr><td colspan='5' style='text-align: right;' class='bold'>ИТОГО К ОПЛАТЕ:</td><td class='bold'>{r.FinalPrice:0.00} руб.</td></tr>");
        sb.Append("</table>");

        sb.Append(@"
            <div class='footer-text'>
                <b>Условия предоставления гарантии:</b><br/>
                1. Исполнитель предоставляет гарантию на выполненные работы с даты подписания акта.<br/>
                2. Гарантия не распространяется на механические повреждения, следы влаги и программные сбои.<br/>
                3. Заказчик согласен, что все неисправности, обнаруженные при обслуживании, возникли до сдачи оборудования.
            </div>
        ");

        sb.Append($@"
            <table style='border: none; margin-top: 40px;'>
                <tr>
                    <td style='border: none; width: 50%;'><b>Исполнитель:</b> ___________________ / {r.Master?.Name ?? "Менеджер СЦ"}</td>
                    <td style='border: none; width: 50%;'><b>Заказчик:</b> ___________________ / {r.ContactName}<br/><span style='font-size: 10px; margin-left: 70px;'>С условиями ознакомлен и согласен</span></td>
                </tr>
                <tr>
                    <td style='border: none;' colspan='2'><b>Дата:</b> {DateTime.Now:dd.MM.yyyy}</td>
                </tr>
            </table>
        ");

        return sb.ToString();
    }

    public static string GenerateWarrantyHtml(Request r)
    {
        var sb = new StringBuilder();

        sb.Append(@"
            <style>
                body { font-family: Arial, sans-serif; font-size: 14px; color: #000; }
                h2 { text-align: center; margin-bottom: 20px; }
                table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }
                th, td { border: 1px solid #000; padding: 8px; text-align: left; }
                th { background-color: #f8f9fa; font-weight: bold; }
                .footer-text { font-size: 12px; margin-top: 20px; }
            </style>");

        sb.Append(
            $"<h2>Гарантийный талон № {r.Id.ToString().Substring(0, 8)}<br/><span style='font-size: 14px; font-weight: normal;'>от {r.ClosedAt?.ToLocalTime():dd.MM.yyyy}</span></h2>");
        sb.Append($"<p><b>Клиент:</b> {r.ContactName} ({r.ContactPhoneNumber})</p>");
        sb.Append($"<p><b>Оборудование:</b> {r.DeviceModelName} (S/N: {r.DeviceSerialNumber})</p>");

        sb.Append("<table>");
        sb.Append(
            "<tr><th>№</th><th>Позиция</th><th>Гарантия</th><th>Цена</th><th>Скидка</th><th>Количество</th><th>Сумма</th></tr>");

        int index = 1;

        var warrantyServices = r.Services
            .Where(s => s.WarrantyPeriodMonths.HasValue && s.WarrantyPeriodMonths.Value > 0).ToList();

        if (warrantyServices.Any())
        {
            foreach (var service in warrantyServices)
            {
                decimal proportionalDiscount = r.BasePrice > 0
                    ? (service.Price / r.BasePrice) * r.AppliedDiscounts.Sum(d => d.SavedAmount)
                    : 0;
                decimal finalServicePrice = service.Price - proportionalDiscount;

                sb.Append("<tr>");
                sb.Append($"<td>{index++}</td>");
                sb.Append($"<td>{service.ServiceName}</td>");
                sb.Append($"<td>До {service.WarrantyEndDate?.ToLocalTime():dd.MM.yyyy}</td>");
                sb.Append($"<td>{service.Price:0.00}</td>");
                sb.Append($"<td>{proportionalDiscount:0.00}</td>");
                sb.Append($"<td>1</td>");
                sb.Append($"<td>{finalServicePrice:0.00}</td>");
                sb.Append("</tr>");
            }
        }
        else
        {
            sb.Append("<tr><td colspan='7' style='text-align: center;'>Гарантийных позиций нет</td></tr>");
        }

        sb.Append(
            $"<tr><td colspan='6' style='text-align: right;'><b>Итого по чеку:</b></td><td><b>{r.FinalPrice:0.00} руб.</b></td></tr>");
        sb.Append("</table>");

        sb.Append(@"
            <div class='footer-text'>
                <b>Гарантийные условия:</b><br/>
                1. Товар прошел контроль качества и признан исправным.<br/>
                2. Гарантия действует при соблюдении правил эксплуатации.<br/>
                3. Гарантия не распространяется на следы влаги и механические повреждения.<br/>
            </div>
            <p style='margin-top: 40px;'><b>Сотрудник СЦ:</b> ___________________ / " + (r.Master?.Name ?? "Менеджер") +
                  "</p>");

        return sb.ToString();
    }

    public static string GenerateAcceptanceHtml(Domain.Aggregates.Request r)
    {
        var sb = new StringBuilder();

        sb.Append(@"
        <style>
            body { font-family: Arial, sans-serif; font-size: 14px; color: #000; }
            h1 { font-size: 24px; margin-bottom: 5px; }
            table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }
            th, td { border: 1px solid #000; padding: 10px; text-align: left; }
            .bold { font-weight: bold; width: 35%; background-color: #f8f9fa; }
            .footer-text { font-size: 11px; margin-top: 20px; line-height: 1.5; text-align: justify; }
        </style>");

        sb.Append($"<h1>Приемная квитанция (Акт передачи)</h1>");
        sb.Append($"<p>Заказ № {r.Id.ToString().Substring(0, 8)} от {r.CreatedAt.ToLocalTime():dd.MM.yyyy HH:mm}</p>");
        sb.Append($"<h3>1. Предмет передачи</h3>");
        sb.Append($"<p>Исполнитель принял, а Заказчик передал следующие объекты оборудования:</p>");

        sb.Append("<table>");
        sb.Append($"<tr><td class='bold'>Клиент</td><td>{r.ContactName}, тел. {r.ContactPhoneNumber}</td></tr>");
        sb.Append($"<tr><td class='bold'>Устройство</td><td>{r.DeviceType?.Name}, {r.DeviceModelName}</td></tr>");
        sb.Append(
            $"<tr><td class='bold'>Серийный номер / IMEI</td><td>{(string.IsNullOrWhiteSpace(r.DeviceSerialNumber) ? "Не указан" : r.DeviceSerialNumber)}</td></tr>");
        sb.Append(
            $"<tr><td class='bold'>Внешний вид и состояние</td><td>{(string.IsNullOrWhiteSpace(r.DeviceAppearance) ? "Б/У, возможны скрытые повреждения" : r.DeviceAppearance)}</td></tr>");
        sb.Append(
            $"<tr><td class='bold'>Комплектация</td><td>{(string.IsNullOrWhiteSpace(r.DevicePackage) ? "Без комплекта" : r.DevicePackage)}</td></tr>");
        sb.Append($"<tr><td class='bold'>Причина обращения (жалоба)</td><td>{r.Description}</td></tr>");

        string priceText = r.BasePrice > 0
            ? $"{r.BasePrice:0.00} руб. (ориентировочно)"
            : "Будет согласована после диагностики";
        sb.Append($"<tr><td class='bold'>Предварительная стоимость</td><td>{priceText}</td></tr>");
        sb.Append("</table>");

        sb.Append(@"
        <h3>2. Условия обслуживания</h3>
        <div class='footer-text'>
            1. Клиент принимает на себя риск возможной полной или частичной утраты работоспособности устройства в процессе ремонта в случае грубых нарушений условий эксплуатации (попадание жидкости, сильные удары).<br/>
            2. Сервисный центр не несет ответственности за возможную потерю данных в памяти устройства, а также за оставленные SIM и Flash-карты.<br/>
            3. Исполнитель обязуется произвести диагностику и ремонт исключительно заявленной Клиентом неисправности. В случае выявления скрытых дефектов стоимость согласуется дополнительно.<br/>
            4. В случае отказа от ремонта Клиент оплачивает стоимость диагностики согласно прейскуранту.<br/>
            5. Аппарат выдается только при предъявлении настоящей квитанции или паспорта заказчика.
        </div>
    ");

        sb.Append($@"
        <table style='border: none; margin-top: 50px;'>
            <tr>
                <td style='border: none; width: 50%;'><b>Исполнитель (Принял):</b> _______________ / {r.Master?.Name ?? "Менеджер СЦ"}</td>
                <td style='border: none; width: 50%;'><b>Заказчик (Сдал):</b> _______________ / {r.ContactName}</td>
            </tr>
        </table>
    ");

        return sb.ToString();
    }

    public static string GenerateReportHtml(string periodText, RequestsStatsDto requestsKpis, ClientsStatsDto clientsStats, MastersStatsDto mastersStats)
    {
        var sb = new StringBuilder();

        sb.Append(@"
            <style>
                body { font-family: Arial, sans-serif; font-size: 14px; color: #333; line-height: 1.6; }
                h1 { text-align: center; color: #111827; margin-bottom: 5px; }
                .subtitle { text-align: center; color: #6b7280; margin-bottom: 30px; font-size: 16px; }
                h2 { border-bottom: 2px solid #3b82f6; color: #1e40af; padding-bottom: 5px; margin-top: 30px; }
                table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }
                th, td { border: 1px solid #e5e7eb; padding: 10px; text-align: left; }
                th { background-color: #f9fafb; width: 60%; font-weight: normal; color: #4b5563; }
                td { font-weight: bold; color: #111827; }
                .revenue { color: #16a34a; font-size: 16px; }
            </style>");

        sb.Append("<h1>Сводный аналитический отчет о работе сервисного центра</h1>");
        sb.Append($"<div class='subtitle'>Отчетный период: {periodText}<br/>Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}</div>");

        sb.Append("<h2>1. Заявки и финансовые показатели</h2>");
        sb.Append("<table>");
        sb.Append($"<tr><th>Всего поступило заявок:</th><td>{requestsKpis.TotalRequests} шт.</td></tr>");
        sb.Append($"<tr><th>Успешно закрыто (выполнен ремонт):</th><td style='color: #16a34a;'>{requestsKpis.ClosedRequests} шт.</td></tr>");
        sb.Append($"<tr><th>Отменено (отказы):</th><td style='color: #dc2626;'>{requestsKpis.CancelledRequests} шт.</td></tr>");
        sb.Append($"<tr><th>Среднее время выполнения ремонта:</th><td>{requestsKpis.AverageRepairTimeHours} ч.</td></tr>");
        sb.Append($"<tr><th>Средний чек:</th><td>{requestsKpis.AverageCheck:0.00} руб.</td></tr>");
        sb.Append($"<tr><th>Общая выручка:</th><td class='revenue'>{requestsKpis.TotalRevenue:0.00} руб.</td></tr>");
        sb.Append("</table>");

        sb.Append("<h2>2. Клиенты и лояльность</h2>");
        sb.Append("<table>");
        sb.Append($"<tr><th>Новых клиентов за период:</th><td>{clientsStats.NewClientsCount} чел.</td></tr>");
        sb.Append($"<tr><th>Заявок от постоянных клиентов (Retention):</th><td>{clientsStats.ReturningClientRequestsCount} шт.</td></tr>");
        sb.Append($"<tr><th>Средняя оценка качества обслуживания:</th><td style='color: #d97706;'>★ {clientsStats.AverageRating:0.0} / 5.0</td></tr>");
        sb.Append("</table>");

        sb.Append("<h2>3. Эффективность персонала (Мастера)</h2>");
        sb.Append("<table>");
        sb.Append($"<tr><th>Задействовано мастеров в периоде:</th><td>{mastersStats.ActiveMastersCount} чел.</td></tr>");
        sb.Append($"<tr><th>Топ-мастер (наибольшее кол-во ремонтов):</th><td>{mastersStats.TopMasterName}</td></tr>");
        sb.Append($"<tr><th>Среднее время диагностики:</th><td>{mastersStats.AverageDiagnosticTimeHours} ч.</td></tr>");
        sb.Append("</table>");

        if (mastersStats.RevenueByMaster.Any())
        {
            sb.Append("<h3>Выручка в разрезе мастеров:</h3>");
            sb.Append("<table>");
            sb.Append("<tr><th style='background-color: #e5e7eb; font-weight: bold;'>ФИО Мастера</th><th style='background-color: #e5e7eb; font-weight: bold;'>Сгенерированная выручка</th></tr>");
            foreach (var master in mastersStats.RevenueByMaster.OrderByDescending(x => x.Value))
            {
                sb.Append($"<tr><td style='font-weight: normal;'>{master.Key}</td><td>{master.Value:0.00} руб.</td></tr>");
            }
            sb.Append("</table>");
        }

        return sb.ToString();
    }
}