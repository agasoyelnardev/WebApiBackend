using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Events;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Movies.Events;

public class RecalculateMovieRatingHandler
    : INotificationHandler<MovieRatingChangedEvent>
{
    private readonly IAppDbContext _context;

    public RecalculateMovieRatingHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(MovieRatingChangedEvent notification, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == notification.MovieId, cancellationToken);

        if (movie is null)
            return; // Film silinmiş ola bilər, sakitcə çıx

        var hasReviews = await _context.Reviews
            .AnyAsync(r => r.MovieId == notification.MovieId, cancellationToken);

        movie.Rating = hasReviews
            ? await _context.Reviews
                .Where(r => r.MovieId == notification.MovieId)
                .AverageAsync(r => r.Rating, cancellationToken)
            : 0;

        await _context.SaveChangesAsync(cancellationToken);
    }
}