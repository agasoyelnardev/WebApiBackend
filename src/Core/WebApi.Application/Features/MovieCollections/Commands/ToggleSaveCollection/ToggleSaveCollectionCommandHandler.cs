using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieCollections.Commands.ToggleSaveCollection;

public class ToggleSaveCollectionCommandHandler : IRequestHandler<ToggleSaveCollectionCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleSaveCollectionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleSaveCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.MovieCollections
            .FirstOrDefaultAsync(x => x.Id == request.MovieCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.AppUserId == request.UserId)
            throw new BadRequestException("Öz kolleksiyanızı saxlaya bilməzsiniz.");

        if (!collection.IsPublic)
            throw new UnauthorizedAccessException("Bu kolleksiya şəxsidir.");

        var existing = await _context.SavedMovieCollections.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.MovieCollectionId == request.MovieCollectionId,
            cancellationToken);

        if (existing is not null)
        {
            _context.SavedMovieCollections.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var saved = new SavedMovieCollection
        {
            UserId = request.UserId,
            MovieCollectionId = request.MovieCollectionId
        };

        await _context.SavedMovieCollections.AddAsync(saved, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}