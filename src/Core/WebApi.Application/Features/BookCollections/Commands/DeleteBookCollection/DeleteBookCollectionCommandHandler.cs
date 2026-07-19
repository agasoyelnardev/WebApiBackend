using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Commands.DeleteBookCollection;

public class DeleteBookCollectionCommandHandler : IRequestHandler<DeleteBookCollectionCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteBookCollectionCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteBookCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.BookCollections
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (collection.UserId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu kolleksiyanı silmək hüququnuz yoxdur.");

        var items = await _context.BookCollectionItems
            .Where(x => x.BookCollectionId == request.Id)
            .ToListAsync(cancellationToken);

        _context.BookCollectionItems.RemoveRange(items);
        _context.BookCollections.Remove(collection);

        await _context.SaveChangesAsync(cancellationToken);
    }
}