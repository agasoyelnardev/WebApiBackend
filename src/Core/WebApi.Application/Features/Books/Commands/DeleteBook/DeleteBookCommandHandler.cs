using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeleteBookCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (book is null)
            throw new NotFoundException("Kitab tapılmadı.");

        var relatedReviews = await _context.BookReviews
            .Where(r => r.BookId == request.Id)
            .ToListAsync(cancellationToken);

        _context.BookReviews.RemoveRange(relatedReviews);

        book.IsDeleted = true;
        book.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}