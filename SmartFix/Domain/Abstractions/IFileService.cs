namespace SmartFix.Domain.Abstractions;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken = default);
}