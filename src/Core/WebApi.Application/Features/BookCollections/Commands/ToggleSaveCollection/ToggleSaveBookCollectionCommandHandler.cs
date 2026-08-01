using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookCollections.Commands.ToggleSaveCollection;

public class ToggleSaveBookCollectionCommandHandler : IRequestHandler<ToggleSaveBookCollectionCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;   

    public ToggleSaveBookCollectionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleSaveBookCollectionCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.BookCollections
            .FirstOrDefaultAsync(x => x.Id == request.BookCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.UserId == currentUserId)   
            throw new BadRequestException("Öz kolleksiyanızı saxlaya bilməzsiniz.");

        var existing = await _context.SavedBookCollections.FirstOrDefaultAsync(
            x => x.UserId == currentUserId && x.BookCollectionId == request.BookCollectionId,   // ← dəyişdi
            cancellationToken);

        if (existing is not null)
        {
            _context.SavedBookCollections.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var saved = new SavedBookCollection
        {
            UserId = currentUserId,   
            BookCollectionId = request.BookCollectionId
        };

        await _context.SavedBookCollections.AddAsync(saved, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}