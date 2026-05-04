namespace SmartFix.Application.Features.Documents.DTO;

public class DocumentDto
{
    public byte[] FileContents { get; set; } = [];
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}