using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookCollections.Commands.ToggleCollectionLike;

public class ToggleBookCollectionLikeCommandHandler : IRequestHandler<ToggleBookCollectionLikeCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleBookCollectionLikeCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleBookCollectionLikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collectionExists = await _context.BookCollections
            .AnyAsync(c => c.Id == request.BookCollectionId, cancellationToken);

        if (!collectionExists)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var existing = await _context.BookCollectionLikes.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.BookCollectionId == request.BookCollectionId,
            cancellationToken);

        if (existing is not null)
        {
            _context.BookCollectionLikes.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new BookCollectionLike
        {
            UserId = request.UserId,
            BookCollectionId = request.BookCollectionId
        };

        await _context.BookCollectionLikes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}