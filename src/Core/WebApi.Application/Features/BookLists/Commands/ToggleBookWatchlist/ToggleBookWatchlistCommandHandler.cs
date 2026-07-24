using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookLists.Commands.ToggleBookWatchlist;

public class ToggleBookWatchlistCommandHandler : IRequestHandler<ToggleBookWatchlistCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleBookWatchlistCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleBookWatchlistCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var bookExists = await _context.Books
            .AnyAsync(b => b.Id == request.BookId && !b.IsDeleted, cancellationToken);

        if (!bookExists)
            throw new NotFoundException("Kitab tapılmadı.");

        var existing = await _context.UserBookWatchlistItems.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.BookId == request.BookId,
            cancellationToken);

        if (existing is not null)
        {
            _context.UserBookWatchlistItems.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var item = new UserBookWatchlistItem
        {
            UserId = request.UserId,
            BookId = request.BookId
        };

        await _context.UserBookWatchlistItems.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}