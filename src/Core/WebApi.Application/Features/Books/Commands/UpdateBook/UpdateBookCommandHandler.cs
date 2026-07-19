using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
{
    private static readonly string[] AllowedLanguages = { "az", "en" };

    private readonly IAppDbContext _context;

    public UpdateBookCommandHandler(IAppDbContext context)
    {
        _context = context;
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

        book.Title = request.Title;
        book.Author = request.Author;
        book.Description = request.Description;
        book.Cover = request.Cover;
        book.Language = request.Language;
        book.Year = request.Year;
        book.Pages = request.Pages;
        book.DownloadUrl = request.DownloadUrl;
        book.PdfUrl = request.PdfUrl;
        book.CustomContent = request.CustomContent;
        book.IsTrending = request.IsTrending;
        book.IsTopRated = request.IsTopRated;
        book.IsNewRelease = request.IsNewRelease;
        
        book.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}