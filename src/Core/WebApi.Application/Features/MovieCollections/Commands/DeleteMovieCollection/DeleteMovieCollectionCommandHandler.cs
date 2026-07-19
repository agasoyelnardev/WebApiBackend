using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieCollections.Commands.DeleteMovieCollection;

public class DeleteMovieCollectionCommandHandler : IRequestHandler<DeleteMovieCollectionCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteMovieCollectionCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteMovieCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.MovieCollections
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (collection.AppUserId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu kolleksiyanı silmək hüququnuz yoxdur.");

        var items = await _context.MovieCollectionItems
            .Where(x => x.MovieCollectionId == request.Id)
            .ToListAsync(cancellationToken);

        _context.MovieCollectionItems.RemoveRange(items);
        _context.MovieCollections.Remove(collection);

        await _context.SaveChangesAsync(cancellationToken);
    }
}