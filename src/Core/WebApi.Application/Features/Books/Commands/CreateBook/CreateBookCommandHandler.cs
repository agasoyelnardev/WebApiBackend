using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
{
    private static readonly string[] AllowedLanguages = { "az", "en" };

    private readonly IAppDbContext _context;

    public CreateBookCommandHandler(IAppDbContext context)
    {
        _context = context;
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

        if (string.IsNullOrWhiteSpace(request.DownloadUrl)
            && string.IsNullOrWhiteSpace(request.PdfUrl)
            && string.IsNullOrWhiteSpace(request.CustomContent))
            throw new BadRequestException("Kitabın oxunması üçün ən azı bir mənbə (DownloadUrl, PdfUrl və ya CustomContent) təqdim edilməlidir.");

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
            PdfUrl = request.PdfUrl,
            CustomContent = request.CustomContent,
            IsTrending = request.IsTrending,
            IsTopRated = request.IsTopRated,
            IsNewRelease = request.IsNewRelease
        };

        await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}