using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieCollections.Commands.RemoveMovieFromCollection;

public class RemoveMovieFromCollectionCommandHandler : IRequestHandler<RemoveMovieFromCollectionCommand>
{
    private readonly IAppDbContext _context;

    public RemoveMovieFromCollectionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RemoveMovieFromCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.MovieCollections
            .FirstOrDefaultAsync(x => x.Id == request.MovieCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.AppUserId != request.RequestedByUserId)
            throw new UnauthorizedAccessException("Bu kolleksiyadan film çıxarmaq hüququnuz yoxdur.");

        var item = await _context.MovieCollectionItems
            .FirstOrDefaultAsync(
                x => x.MovieCollectionId == request.MovieCollectionId && x.MovieId == request.MovieId,
                cancellationToken);

        if (item is null)
            throw new NotFoundException("Bu film kolleksiyada tapılmadı.");

        _context.MovieCollectionItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }
}