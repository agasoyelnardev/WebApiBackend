using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookLists.Commands.ToggleBookLike;

public class ToggleBookLikeCommandHandler : IRequestHandler<ToggleBookLikeCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleBookLikeCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleBookLikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == request.BookId && !b.IsDeleted, cancellationToken);

        if (book is null)
            throw new NotFoundException("Kitab tapılmadı.");

        var existing = await _context.BookLikes.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.BookId == request.BookId,
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
            UserId = request.UserId,
            BookId = request.BookId
        };

        await _context.BookLikes.AddAsync(like, cancellationToken);
        book.Likes++;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}