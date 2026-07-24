using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieLists.Commands.ToggleMovieLike;

public class ToggleMovieLikeCommandHandler : IRequestHandler<ToggleMovieLikeCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleMovieLikeCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleMovieLikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == request.MovieId && !m.IsDeleted, cancellationToken);

        if (movie is null)
            throw new NotFoundException("Film tapılmadı.");

        var existing = await _context.MovieLikes.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.MovieId == request.MovieId,
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
            UserId = request.UserId,
            MovieId = request.MovieId
        };

        await _context.MovieLikes.AddAsync(like, cancellationToken);
        movie.Likes++;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}