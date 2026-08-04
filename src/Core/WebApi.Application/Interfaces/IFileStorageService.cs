namespace WebApi.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SavePdfAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
}