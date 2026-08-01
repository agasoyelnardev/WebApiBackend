using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookCollections.Commands.ToggleCollectionLike;

public class ToggleBookCollectionLikeCommandHandler : IRequestHandler<ToggleBookCollectionLikeCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;   

    public ToggleBookCollectionLikeCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleBookCollectionLikeCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collectionExists = await _context.BookCollections
            .AnyAsync(c => c.Id == request.BookCollectionId, cancellationToken);

        if (!collectionExists)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var existing = await _context.BookCollectionLikes.FirstOrDefaultAsync(
            x => x.UserId == currentUserId && x.BookCollectionId == request.BookCollectionId,   // ← dəyişdi
            cancellationToken);

        if (existing is not null)
        {
            _context.BookCollectionLikes.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new BookCollectionLike
        {
            UserId = currentUserId,   
            BookCollectionId = request.BookCollectionId
        };

        await _context.BookCollectionLikes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}