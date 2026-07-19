using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieCollections.Commands.AddMovieToCollection;

public class AddMovieToCollectionCommandHandler : IRequestHandler<AddMovieToCollectionCommand>
{
    private readonly IAppDbContext _context;

    public AddMovieToCollectionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(AddMovieToCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.MovieCollections
            .FirstOrDefaultAsync(x => x.Id == request.MovieCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.AppUserId != request.RequestedByUserId)
            throw new UnauthorizedAccessException("Bu kolleksiyaya film əlavə etmək hüququnuz yoxdur.");

        var movieExists = await _context.Movies
            .AnyAsync(x => x.Id == request.MovieId && !x.IsDeleted, cancellationToken);

        if (!movieExists)
            throw new NotFoundException("Film tapılmadı.");

        var alreadyAdded = await _context.MovieCollectionItems
            .AnyAsync(x => x.MovieCollectionId == request.MovieCollectionId && x.MovieId == request.MovieId,
                cancellationToken);

        if (alreadyAdded)
            throw new ConflictException("Bu film artıq kolleksiyaya əlavə edilib.");

        var item = new MovieCollectionItem
        {
            MovieCollectionId = request.MovieCollectionId,
            MovieId = request.MovieId
        };

        await _context.MovieCollectionItems.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}