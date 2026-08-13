using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Commands.RemoveBookFromCollection;

public class RemoveBookFromCollectionCommandHandler : IRequestHandler<RemoveBookFromCollectionCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;   

    public RemoveBookFromCollectionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(RemoveBookFromCollectionCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.BookCollections
            .FirstOrDefaultAsync(x => x.Id == request.BookCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.UserId != currentUserId)  
            throw new UnauthorizedAccessException("Bu kolleksiyadan kitab çıxarmaq hüququnuz yoxdur.");

        var item = await _context.BookCollectionItems
            .FirstOrDefaultAsync(
                x => x.BookCollectionId == request.BookCollectionId && x.BookId == request.BookId,
                cancellationToken);

        if (item is null)
            throw new NotFoundException("Bu kitab kolleksiyada tapılmadı.");

        _context.BookCollectionItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }
}