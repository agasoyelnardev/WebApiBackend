using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Events;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookReviews.Commands.UpdateBookReview;

public class UpdateBookReviewCommandHandler : IRequestHandler<UpdateBookReviewCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublisher _publisher;

    public UpdateBookReviewCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IPublisher publisher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _publisher = publisher;
    }

    public async Task Handle(UpdateBookReviewCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Comment))
            throw new BadRequestException("Rəy boş ola bilməz.");

        if (request.Comment.Length > 1000)
            throw new BadRequestException("Rəy maksimum 1000 simvol ola bilər.");

        if (request.Rating < 1 || request.Rating > 5)
            throw new BadRequestException("Reytinq 1 ilə 5 arasında olmalıdır.");

        var review = await _context.BookReviews
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (review is null)
            throw new NotFoundException("Rəy tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (review.UserId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu rəyi yeniləmək hüququnuz yoxdur.");

        review.Rating = request.Rating;
        review.Comment = request.Comment;
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new BookRatingChangedEvent(review.BookId), cancellationToken);
    }
}