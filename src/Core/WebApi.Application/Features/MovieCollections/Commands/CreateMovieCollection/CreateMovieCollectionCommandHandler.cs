using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieCollections.Commands.CreateMovieCollection;

public class CreateMovieCollectionCommandHandler : IRequestHandler<CreateMovieCollectionCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateMovieCollectionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMovieCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AppUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new BadRequestException("Kolleksiya adı boş ola bilməz.");

        if (request.Name.Length > 150)
            throw new BadRequestException("Kolleksiya adı maksimum 150 simvol ola bilər.");

        var collection = new MovieCollection
        {
            Name = request.Name,
            Description = request.Description,
            CoverImageUrl = request.CoverImageUrl,
            IsPublic = request.IsPublic,
            AppUserId = request.AppUserId
        };

        await _context.MovieCollections.AddAsync(collection, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return collection.Id;
    }
}