using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookLists.Commands.ToggleBookFavorite;

public class ToggleBookFavoriteCommandHandler : IRequestHandler<ToggleBookFavoriteCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleBookFavoriteCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleBookFavoriteCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var bookExists = await _context.Books
            .AnyAsync(b => b.Id == request.BookId && !b.IsDeleted, cancellationToken);

        if (!bookExists)
            throw new NotFoundException("Kitab tapılmadı.");

        var existing = await _context.UserBookFavorites.FirstOrDefaultAsync(
            x => x.UserId == currentUserId && x.BookId == request.BookId,
            cancellationToken);

        if (existing is not null)
        {
            _context.UserBookFavorites.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var item = new UserBookFavorite
        {
            UserId = currentUserId,
            BookId = request.BookId
        };

        await _context.UserBookFavorites.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}