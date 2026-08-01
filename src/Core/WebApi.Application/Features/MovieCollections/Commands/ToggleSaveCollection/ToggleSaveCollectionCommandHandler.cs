using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieCollections.Commands.ToggleSaveCollection;

public class ToggleSaveCollectionCommandHandler : IRequestHandler<ToggleSaveCollectionCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleSaveCollectionCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        ToggleSaveCollectionCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.MovieCollections
            .FirstOrDefaultAsync(x => x.Id == request.MovieCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.AppUserId == _currentUserService.UserId)
            throw new BadRequestException("Öz kolleksiyanızı saxlaya bilməzsiniz.");

        if (!collection.IsPublic)
            throw new UnauthorizedAccessException("Bu kolleksiya şəxsidir.");

        var existing = await _context.SavedMovieCollections
            .FirstOrDefaultAsync(
                x => x.UserId == _currentUserService.UserId &&
                     x.MovieCollectionId == request.MovieCollectionId,
                cancellationToken);

        if (existing is not null)
        {
            _context.SavedMovieCollections.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var saved = new SavedMovieCollection
        {
            UserId = _currentUserService.UserId,
            MovieCollectionId = request.MovieCollectionId
        };

        await _context.SavedMovieCollections.AddAsync(saved, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}