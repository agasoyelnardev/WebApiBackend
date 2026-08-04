using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
{
    private static readonly string[] AllowedLanguages = { "az", "en" };
    private const long MaxFileSizeBytes = 50 * 1024 * 1024; // 50MB

    private readonly IAppDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public CreateBookCommandHandler(IAppDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Kitab adı boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.Author))
            throw new BadRequestException("Müəllif adı boş ola bilməz.");

        if (request.Year < 1000 || request.Year > DateTime.UtcNow.Year)
            throw new BadRequestException("Nəşr ili düzgün deyil.");

        if (request.Pages <= 0)
            throw new BadRequestException("Səhifə sayı müsbət olmalıdır.");

        if (!AllowedLanguages.Contains(request.Language))
            throw new BadRequestException("Dil yalnız 'az' və ya 'en' ola bilər.");

        // PDF faylı yüklənibsə, onu diskə saxla və PdfUrl-i avtomatik təyin et
        var pdfUrl = request.PdfUrl;

        if (request.PdfFile is not null && request.PdfFile.Length > 0)
        {
            pdfUrl = await ValidateAndSavePdfAsync(request.PdfFile, cancellationToken);
        }

        if (string.IsNullOrWhiteSpace(request.DownloadUrl)
            && string.IsNullOrWhiteSpace(pdfUrl)
            && string.IsNullOrWhiteSpace(request.CustomContent))
            throw new BadRequestException("Kitabın oxunması üçün ən azı bir mənbə (DownloadUrl, PdfUrl/PdfFile və ya CustomContent) təqdim edilməlidir.");

        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Description = request.Description,
            Cover = request.Cover,
            Rating = 0,
            Language = request.Language,
            Year = request.Year,
            Pages = request.Pages,
            DownloadUrl = request.DownloadUrl,
            PdfUrl = pdfUrl,
            CustomContent = request.CustomContent,
            IsTrending = request.IsTrending,
            IsTopRated = request.IsTopRated,
            IsNewRelease = request.IsNewRelease,
            Genres = request.Genres,
        };

        await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }

    private async Task<string> ValidateAndSavePdfAsync(Microsoft.AspNetCore.Http.IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length > MaxFileSizeBytes)
            throw new BadRequestException("Fayl ölçüsü maksimum 50MB ola bilər.");

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
            throw new BadRequestException("Yalnız PDF faylları qəbul olunur.");

        if (!string.Equals(file.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            throw new BadRequestException("Fayl formatı düzgün deyil.");

        await using var stream = file.OpenReadStream();

        var buffer = new byte[4];
        var bytesRead = await stream.ReadAsync(buffer, 0, 4, cancellationToken);
        var header = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);

        if (header != "%PDF")
            throw new BadRequestException("Fayl real PDF formatında deyil.");

        stream.Position = 0;

        return await _fileStorageService.SavePdfAsync(stream, file.FileName, cancellationToken);
    }
}