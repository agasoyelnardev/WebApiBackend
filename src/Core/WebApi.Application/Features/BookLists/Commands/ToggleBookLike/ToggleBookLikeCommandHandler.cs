using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookLists.Commands.ToggleBookLike;

public class ToggleBookLikeCommandHandler : IRequestHandler<ToggleBookLikeCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleBookLikeCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleBookLikeCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == request.BookId && !b.IsDeleted, cancellationToken);

        if (book is null)
            throw new NotFoundException("Kitab tapılmadı.");

        var existing = await _context.BookLikes.FirstOrDefaultAsync(
            x => x.UserId == currentUserId && x.BookId == request.BookId,
            cancellationToken);

        if (existing is not null)
        {
            _context.BookLikes.Remove(existing);
            book.Likes = Math.Max(0, book.Likes - 1);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new BookLike
        {
            UserId = currentUserId,
            BookId = request.BookId
        };

        await _context.BookLikes.AddAsync(like, cancellationToken);
        book.Likes++;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}