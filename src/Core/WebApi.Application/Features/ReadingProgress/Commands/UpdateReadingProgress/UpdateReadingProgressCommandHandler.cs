using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.ReadingProgress.Commands.UpdateReadingProgress;

public class UpdateReadingProgressCommandHandler : IRequestHandler<UpdateReadingProgressCommand>
{
    private readonly IAppDbContext _context;
    private readonly IPointsService _pointsService;
    private readonly ICurrentUserService _currentUserService;

    public UpdateReadingProgressCommandHandler(
        IAppDbContext context,
        IPointsService pointsService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _pointsService = pointsService;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateReadingProgressCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (request.PercentageComplete < 0 || request.PercentageComplete > 100)
            throw new BadRequestException("Faiz dəyəri 0 ilə 100 arasında olmalıdır.");

        var bookExists = await _context.Books
            .AnyAsync(b => b.Id == request.BookId && !b.IsDeleted, cancellationToken);

        if (!bookExists)
            throw new NotFoundException("Kitab tapılmadı.");

        var progress = await _context.ReadingProgresses.FirstOrDefaultAsync(
            x => x.UserId == currentUserId && x.BookId == request.BookId,
            cancellationToken);

        var previousPercentage = progress?.PercentageComplete ?? 0;

        if (progress is null)
        {
            progress = new Domain.Entities.ReadingProgress
            {
                UserId = currentUserId,
                BookId = request.BookId,
                PercentageComplete = request.PercentageComplete
            };

            await _context.ReadingProgresses.AddAsync(progress, cancellationToken);
        }
        else
        {
            progress.PercentageComplete = request.PercentageComplete;
            progress.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (previousPercentage < 100 && request.PercentageComplete >= 100)
        {
            await _pointsService.AwardPointsAsync(currentUserId, PointAction.ReadingProgress100, cancellationToken);
        }
        else if (previousPercentage < 50 && request.PercentageComplete >= 50)
        {
            await _pointsService.AwardPointsAsync(currentUserId, PointAction.ReadingProgress50, cancellationToken);
        }
    }
}