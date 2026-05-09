using MediatR;
using SelectPdf;
using SmartFix.Application.Features.Documents.DTO;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Statistics.Queries.PdfReport;

public class GetPdfReportQueryHandler:IRequestHandler<GetPdfReportQuery, DocumentDto>
{
    private readonly IStatisticsRepository _repository;

    public GetPdfReportQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentDto> Handle(GetPdfReportQuery request, CancellationToken cancellationToken)
    {
        var (startDate, endDate) = DateRangeCalculator.CalculateDateRange(request.Period, request.From, request.To);
        var requestsKpis = await _repository.LoadRequestsKpis(startDate, endDate, cancellationToken);
        var clientsStats = await _repository.LoadClientStats(startDate, endDate, cancellationToken);
        var mastersStats = await _repository.LoadMasterStats(startDate, endDate, cancellationToken);
        
        string periodText = request.Period switch
        {
            "week" => "за последние 7 дней",
            "month" => "за последние 30 дней",
            "year" => "за последний год",
            "custom" => $"с {request.From:dd.MM.yyyy} по {request.To:dd.MM.yyyy}",
            _ => "за выбранный период"
        };
        
        string htmlContent = PdfDocsGenerator.GenerateReportHtml(periodText, requestsKpis, clientsStats, mastersStats);
        
        HtmlToPdf converter = new HtmlToPdf();
        converter.Options.PdfPageSize = PdfPageSize.A4;
        converter.Options.MarginTop = 20;
        converter.Options.MarginBottom = 20;
        converter.Options.MarginLeft = 20;
        converter.Options.MarginRight = 20;
        
        converter.Options.MinPageLoadTime = 5;
        PdfDocument doc = converter.ConvertHtmlString(htmlContent);
        
        byte[] pdfBytes;
        using (var memoryStream = new MemoryStream())
        {
            doc.Save(memoryStream);
            pdfBytes = memoryStream.ToArray();
        }
        doc.Close();

        return new DocumentDto
        {
            FileContents = pdfBytes,
            ContentType = "application/pdf",
            FileName = $"Аналитический_отчет_{DateTime.Now:yyyyMMdd}.pdf"
        };
    }
}