using MediatR;
using SmartFix.Application.Features.Documents.DTO;

namespace SmartFix.Application.Features.Statistics.Queries.PdfReport;

public class GetPdfReportQuery:IRequest<DocumentDto>
{
    public string Period { get; set; } = "month"; 
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}