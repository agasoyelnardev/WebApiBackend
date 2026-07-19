using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieCollections.Commands.UpdateMovieCollection;

public class UpdateMovieCollectionCommandHandler : IRequestHandler<UpdateMovieCollectionCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateMovieCollectionCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateMovieCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new BadRequestException("Kolleksiya adı boş ola bilməz.");

        if (request.Name.Length > 150)
            throw new BadRequestException("Kolleksiya adı maksimum 150 simvol ola bilər.");

        var collection = await _context.MovieCollections
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (collection.AppUserId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu kolleksiyanı redaktə etmək hüququnuz yoxdur.");

        collection.Name = request.Name;
        collection.Description = request.Description;
        collection.CoverImageUrl = request.CoverImageUrl;
        collection.IsPublic = request.IsPublic;
        collection.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}