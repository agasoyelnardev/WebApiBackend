using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookCollections.Commands.UpdateBookCollection;

public class UpdateBookCollectionCommandHandler : IRequestHandler<UpdateBookCollectionCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBookCollectionCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateBookCollectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Kolleksiya adı boş ola bilməz.");

        if (request.Title.Length > 150)
            throw new BadRequestException("Kolleksiya adı maksimum 150 simvol ola bilər.");

        var collection = await _context.BookCollections
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (collection is null)
            throw new NotFoundException("Kolleksiya tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (collection.UserId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu kolleksiyanı redaktə etmək hüququnuz yoxdur.");

        collection.Title = request.Title;
        collection.Description = request.Description;
        collection.Cover = request.Cover;
        collection.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}