using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookCollections.Commands.CreateBookCollection;

public class CreateBookCollectionCommandHandler : IRequestHandler<CreateBookCollectionCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateBookCollectionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateBookCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Kolleksiya adı boş ola bilməz.");

        if (request.Title.Length > 150)
            throw new BadRequestException("Kolleksiya adı maksimum 150 simvol ola bilər.");

        var collection = new BookCollection
        {
            Title = request.Title,
            Description = request.Description,
            Cover = request.Cover,
            UserId = request.UserId
        };

        await _context.BookCollections.AddAsync(collection, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return collection.Id;
    }
}