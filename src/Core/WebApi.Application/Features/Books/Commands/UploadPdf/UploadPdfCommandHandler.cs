using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Commands.UploadPdf;

public class UploadPdfCommandHandler : IRequestHandler<UploadPdfCommand, string>
{
    private const long MaxFileSizeBytes = 50 * 1024 * 1024; // 50MB

    private readonly IFileStorageService _fileStorageService;

    public UploadPdfCommandHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    public async Task<string> Handle(UploadPdfCommand request, CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
            throw new BadRequestException("Fayl seçilməyib.");

        if (request.File.Length > MaxFileSizeBytes)
            throw new BadRequestException("Fayl ölçüsü maksimum 50MB ola bilər.");

        var extension = Path.GetExtension(request.File.FileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
            throw new BadRequestException("Yalnız PDF faylları qəbul olunur.");

        if (!string.Equals(request.File.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            throw new BadRequestException("Fayl formatı düzgün deyil.");

        await using var stream = request.File.OpenReadStream();

        var buffer = new byte[4];
        var bytesRead = await stream.ReadAsync(buffer, 0, 4, cancellationToken);
        var header = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);

        if (header != "%PDF")
            throw new BadRequestException("Fayl real PDF formatında deyil.");

        stream.Position = 0; 

        var pdfUrl = await _fileStorageService.SavePdfAsync(stream, request.File.FileName, cancellationToken);

        return pdfUrl;
    }
}