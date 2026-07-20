using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieLists.Commands.ToggleWatchlist;

public class ToggleWatchlistCommandHandler : IRequestHandler<ToggleWatchlistCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleWatchlistCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleWatchlistCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var movieExists = await _context.Movies
            .AnyAsync(m => m.Id == request.MovieId && !m.IsDeleted, cancellationToken);

        if (!movieExists)
            throw new NotFoundException("Film tapılmadı.");

        var existing = await _context.UserMovieLists.FirstOrDefaultAsync(
            x => x.UserId == request.UserId
                 && x.MovieId == request.MovieId
                 && x.Type == MovieListType.Watchlist,
            cancellationToken);

        if (existing is not null)
        {
            _context.UserMovieLists.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var item = new UserMovieList
        {
            UserId = request.UserId,
            MovieId = request.MovieId,
            Type = MovieListType.Watchlist
        };

        await _context.UserMovieLists.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}