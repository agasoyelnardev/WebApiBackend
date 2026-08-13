using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.MovieCollections.Commands.CreateMovieCollection;

public class CreateMovieCollectionCommandHandler : IRequestHandler<CreateMovieCollectionCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;  

    public CreateMovieCollectionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateMovieCollectionCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
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
            AppUserId = currentUserId   
        };

        await _context.MovieCollections.AddAsync(collection, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return collection.Id;
    }
}