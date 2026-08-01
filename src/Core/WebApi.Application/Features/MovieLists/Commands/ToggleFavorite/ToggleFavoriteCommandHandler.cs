using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieLists.Commands.ToggleFavorite;

public class ToggleFavoriteCommandHandler : IRequestHandler<ToggleFavoriteCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleFavoriteCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var movieExists = await _context.Movies
            .AnyAsync(m => m.Id == request.MovieId && !m.IsDeleted, cancellationToken);

        if (!movieExists)
            throw new NotFoundException("Film tapılmadı.");

        var existing = await _context.UserMovieLists.FirstOrDefaultAsync(
            x => x.UserId == currentUserId
                 && x.MovieId == request.MovieId
                 && x.Type == MovieListType.Favorite,
            cancellationToken);

        if (existing is not null)
        {
            _context.UserMovieLists.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var item = new UserMovieList
        {
            UserId = currentUserId,
            MovieId = request.MovieId,
            Type = MovieListType.Favorite
        };

        await _context.UserMovieLists.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}