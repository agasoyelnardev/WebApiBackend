using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Events;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublisher _publisher;

    public UpdateReviewCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IPublisher publisher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _publisher = publisher;
    }

    public async Task Handle(
        UpdateReviewCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 1000)
            throw new BadRequestException("Rəy boş ola bilməz və ya 1000 simvoldan çox ola bilməz.");

        if (request.Rating < 1 || request.Rating > 5)
            throw new BadRequestException("Reytinq 1 ilə 5 arasında olmalıdır.");

        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (review is null)
            throw new NotFoundException("Review tapılmadı.");

        var currentUserId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (review.UserId != currentUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu rəyi yeniləmək hüququnuz yoxdur.");

        review.Content = request.Content.Trim();
        review.Rating = request.Rating;
        
        await _publisher.Publish(new MovieRatingChangedEvent(review.MovieId), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}