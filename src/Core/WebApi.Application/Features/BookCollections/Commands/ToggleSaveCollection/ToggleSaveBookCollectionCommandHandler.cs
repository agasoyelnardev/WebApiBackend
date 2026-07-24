using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookCollections.Commands.ToggleSaveCollection;

public class ToggleSaveBookCollectionCommandHandler : IRequestHandler<ToggleSaveBookCollectionCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleSaveBookCollectionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleSaveBookCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.BookCollections
            .FirstOrDefaultAsync(x => x.Id == request.BookCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.UserId == request.UserId)
            throw new BadRequestException("Öz kolleksiyanızı saxlaya bilməzsiniz.");

        var existing = await _context.SavedBookCollections.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.BookCollectionId == request.BookCollectionId,
            cancellationToken);

        if (existing is not null)
        {
            _context.SavedBookCollections.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var saved = new SavedBookCollection
        {
            UserId = request.UserId,
            BookCollectionId = request.BookCollectionId
        };

        await _context.SavedBookCollections.AddAsync(saved, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}