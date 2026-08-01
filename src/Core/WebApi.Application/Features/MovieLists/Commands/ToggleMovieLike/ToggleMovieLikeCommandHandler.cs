using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieLists.Commands.ToggleMovieLike;

public class ToggleMovieLikeCommandHandler : IRequestHandler<ToggleMovieLikeCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleMovieLikeCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        ToggleMovieLikeCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var movie = await _context.Movies
            .FirstOrDefaultAsync(
                m => m.Id == request.MovieId && !m.IsDeleted,
                cancellationToken);

        if (movie is null)
            throw new NotFoundException("Film tapılmadı.");

        var existing = await _context.MovieLikes
            .FirstOrDefaultAsync(
                x => x.UserId == currentUserId &&
                     x.MovieId == request.MovieId,
                cancellationToken);

        if (existing is not null)
        {
            _context.MovieLikes.Remove(existing);
            movie.Likes = Math.Max(0, movie.Likes - 1);

            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new MovieLike
        {
            UserId = currentUserId,
            MovieId = request.MovieId
        };

        await _context.MovieLikes.AddAsync(like, cancellationToken);

        movie.Likes++;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}