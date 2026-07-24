using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieCollections.Commands.ToggleCollectionLike;

public class ToggleMovieCollectionLikeCommandHandler : IRequestHandler<ToggleMovieCollectionLikeCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleMovieCollectionLikeCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleMovieCollectionLikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collectionExists = await _context.MovieCollections
            .AnyAsync(c => c.Id == request.MovieCollectionId, cancellationToken);

        if (!collectionExists)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var existing = await _context.MovieCollectionLikes.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.MovieCollectionId == request.MovieCollectionId,
            cancellationToken);

        if (existing is not null)
        {
            _context.MovieCollectionLikes.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new MovieCollectionLike
        {
            UserId = request.UserId,
            MovieCollectionId = request.MovieCollectionId
        };

        await _context.MovieCollectionLikes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}