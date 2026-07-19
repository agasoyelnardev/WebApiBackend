using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookCollections.Commands.AddBookToCollection;

public class AddBookToCollectionCommandHandler : IRequestHandler<AddBookToCollectionCommand>
{
    private readonly IAppDbContext _context;

    public AddBookToCollectionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(AddBookToCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var collection = await _context.BookCollections
            .FirstOrDefaultAsync(x => x.Id == request.BookCollectionId, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        if (collection.UserId != request.RequestedByUserId)
            throw new UnauthorizedAccessException("Bu kolleksiyaya kitab əlavə etmək hüququnuz yoxdur.");

        var bookExists = await _context.Books
            .AnyAsync(x => x.Id == request.BookId && !x.IsDeleted, cancellationToken);

        if (!bookExists)
            throw new NotFoundException("Kitab tapılmadı.");

        var alreadyAdded = await _context.BookCollectionItems
            .AnyAsync(x => x.BookCollectionId == request.BookCollectionId && x.BookId == request.BookId,
                cancellationToken);

        if (alreadyAdded)
            throw new ConflictException("Bu kitab artıq kolleksiyaya əlavə edilib.");

        var item = new BookCollectionItem
        {
            BookCollectionId = request.BookCollectionId,
            BookId = request.BookId
        };

        await _context.BookCollectionItems.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}