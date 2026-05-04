using MediatR;
using SmartFix.Application.Features.Documents.DTO;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Documents.Queries.GetRequestDocuments;

public class GetRequestDocumentsQuery: IRequest<DocumentDto>
{
    public Guid RequestId { get; set; }
    public DocumentType Type { get; set; }
}