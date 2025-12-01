namespace SmartFix.Domain.Aggregates;

public class RequestPhoto
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public Guid RequestId { get; private set; }
    private RequestPhoto() { }
    public static RequestPhoto Create(Guid requestId, string fileName, string filePath)
    {
        return new RequestPhoto
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            FileName = fileName,
            FilePath = filePath
        };
    }
}