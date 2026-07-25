using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Books.Commands.ImportBookFromGoogleBooks;

public class ImportBookFromGoogleBooksCommandHandler : IRequestHandler<ImportBookFromGoogleBooksCommand, Guid>
{
    private readonly IBookImportService _importService;
    private readonly IAppDbContext _context;

    public ImportBookFromGoogleBooksCommandHandler(IBookImportService importService, IAppDbContext context)
    {
        _importService = importService;
        _context = context;
    }

    public async Task<Guid> Handle(ImportBookFromGoogleBooksCommand request, CancellationToken cancellationToken)
    {
        var details = await _importService.GetDetailsAsync(request.GoogleBooksId, cancellationToken);

        if (details is null)
            throw new NotFoundException("Google Books-da bu ID ilə kitab tapılmadı.");

        if (string.IsNullOrWhiteSpace(details.Title))
            throw new BadRequestException("Kitabın başlığı tapılmadı, idxal edilə bilmədi.");

        var book = new Book
        {
            Title = details.Title,
            Author = string.IsNullOrWhiteSpace(details.Author) ? "Naməlum" : details.Author,
            Description = details.Description,
            Cover = details.Cover,
            Rating = 0,
            Language = "en", // Google Books əsasən ingilis dilli nəticələr verir, admin sonra dəyişə bilər
            Year = details.Year == 0 ? DateTime.UtcNow.Year : details.Year,
            Pages = details.Pages == 0 ? 1 : details.Pages, // 0 səhifə mənasız olduğu üçün minimal dəyər
            DownloadUrl = details.PreviewLink, // ola bilər null qalsın, admin sonra doldurar
            PdfUrl = null,
            CustomContent = null
        };

        await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}