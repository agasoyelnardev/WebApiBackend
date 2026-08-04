using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
{
    private static readonly string[] AllowedLanguages = { "az", "en" };
    private const long MaxFileSizeBytes = 50 * 1024 * 1024; // 50MB

    private readonly IAppDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public UpdateBookCommandHandler(IAppDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
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

        var book = await _context.Books
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (book is null)
            throw new NotFoundException("Kitab tapılmadı.");

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

        book.Title = request.Title;
        book.Author = request.Author;
        book.Description = request.Description;
        book.Cover = request.Cover;
        book.Language = request.Language;
        book.Year = request.Year;
        book.Pages = request.Pages;
        book.DownloadUrl = request.DownloadUrl;
        book.PdfUrl = pdfUrl;
        book.CustomContent = request.CustomContent;
        book.IsTrending = request.IsTrending;
        book.IsTopRated = request.IsTopRated;
        book.IsNewRelease = request.IsNewRelease;
        book.Genres = request.Genres;

        book.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<string> ValidateAndSavePdfAsync(IFormFile file, CancellationToken cancellationToken)
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