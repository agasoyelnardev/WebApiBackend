using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Events;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Events;

public class RecalculateBookRatingHandler
    : INotificationHandler<BookRatingChangedEvent>
{
    private readonly IAppDbContext _context;

    public RecalculateBookRatingHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(BookRatingChangedEvent notification, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == notification.BookId, cancellationToken);

        if (book is null)
            return;

        var hasReviews = await _context.BookReviews
            .AnyAsync(r => r.BookId == notification.BookId, cancellationToken);

        book.Rating = hasReviews
            ? await _context.BookReviews
                .Where(r => r.BookId == notification.BookId)
                .AverageAsync(r => r.Rating, cancellationToken)
            : 0;

        await _context.SaveChangesAsync(cancellationToken);
    }
}