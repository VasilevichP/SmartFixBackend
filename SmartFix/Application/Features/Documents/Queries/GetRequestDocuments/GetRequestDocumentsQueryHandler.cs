using System.Net;
using MediatR;
using SelectPdf;
using SmartFix.Application.Features.Documents.DTO;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Documents.Queries.GetRequestDocuments;

public class GetRequestDocumentsQueryHandler : IRequestHandler<GetRequestDocumentsQuery, DocumentDto>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IWebHostEnvironment _environment;

    public GetRequestDocumentsQueryHandler(IRequestRepository requestRepository, IWebHostEnvironment environment)
    {
        _requestRepository = requestRepository;
        _environment = environment;
    }

    public async Task<DocumentDto> Handle(GetRequestDocumentsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (entity == null) throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена");

        string htmlContent = "";
        string fileName = "";

        if (request.Type == DocumentType.Acceptance)
        {
            htmlContent = PdfDocsGenerator.GenerateAcceptanceHtml(entity);
            fileName = $"Приемная_квитанция_{entity.Id.ToString().Substring(0, 8)}.pdf";
        }
        else if (request.Type == DocumentType.Completion)
        {
            htmlContent = PdfDocsGenerator.GenerateCompletionActHtml(entity);
            fileName = $"Акт_выполненных_работ_{entity.Id.ToString().Substring(0, 8)}.pdf";
        }
        else if (request.Type == DocumentType.Warranty)
        {
            if (entity.Status != RequestStatus.Closed)
                throw new HttpException(HttpStatusCode.BadRequest,
                    "Гарантийный талон формируется только после оплаты и закрытия заявки");

            htmlContent = PdfDocsGenerator.GenerateWarrantyHtml(entity);
            fileName = $"Гарантийный_талон_{entity.Id.ToString().Substring(0, 8)}.pdf";
        }

        HtmlToPdf converter = new HtmlToPdf();

        converter.Options.PdfPageSize = PdfPageSize.A4;
        converter.Options.MarginTop = 20;
        converter.Options.MarginBottom = 20;
        converter.Options.MarginLeft = 20;
        converter.Options.MarginRight = 20;

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
            FileName = fileName
        };
    }
}