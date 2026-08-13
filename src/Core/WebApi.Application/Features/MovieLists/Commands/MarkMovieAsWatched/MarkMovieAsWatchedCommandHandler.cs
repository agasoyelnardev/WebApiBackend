using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieLists.Commands.MarkMovieAsWatched;

public class MarkMovieAsWatchedCommandHandler : IRequestHandler<MarkMovieAsWatchedCommand>
{
    private readonly IAppDbContext _context;
    private readonly IPointsService _pointsService;
    private readonly ICurrentUserService _currentUserService;

    public MarkMovieAsWatchedCommandHandler(
        IAppDbContext context,
        IPointsService pointsService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _pointsService = pointsService;
        _currentUserService = currentUserService;
    }

    public async Task Handle(MarkMovieAsWatchedCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var movieExists = await _context.Movies
            .AnyAsync(m => m.Id == request.MovieId && !m.IsDeleted, cancellationToken);

        if (!movieExists)
            throw new NotFoundException("Film tapılmadı.");

        var alreadyWatched = await _context.WatchHistories.AnyAsync(
            x => x.UserId == currentUserId && x.MovieId == request.MovieId,
            cancellationToken);

        if (alreadyWatched)
            return;

        var history = new WatchHistory
        {
            UserId = currentUserId,
            MovieId = request.MovieId
        };

        await _context.WatchHistories.AddAsync(history, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _pointsService.AwardPointsAsync(currentUserId, PointAction.WatchMovie, cancellationToken);
    }
}