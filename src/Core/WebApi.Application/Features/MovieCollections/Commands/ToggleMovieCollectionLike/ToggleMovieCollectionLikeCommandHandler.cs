using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieCollections.Commands.ToggleMovieCollectionLike;

public class ToggleMovieCollectionLikeCommandHandler : IRequestHandler<ToggleMovieCollectionLikeCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleMovieCollectionLikeCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleMovieCollectionLikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collectionExists = await _context.MovieCollections
            .AnyAsync(c => c.Id == request.MovieCollectionId, cancellationToken);

        if (!collectionExists)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var existing = await _context.MovieCollectionLikes.FirstOrDefaultAsync(
            x => x.UserId == _currentUserService.UserId &&
                 x.MovieCollectionId == request.MovieCollectionId,
            cancellationToken);

        if (existing is not null)
        {
            _context.MovieCollectionLikes.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new MovieCollectionLike
        {
            UserId = _currentUserService.UserId,
            MovieCollectionId = request.MovieCollectionId
        };

        await _context.MovieCollectionLikes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}