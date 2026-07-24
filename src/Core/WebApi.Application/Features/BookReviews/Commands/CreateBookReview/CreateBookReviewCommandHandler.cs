using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Events;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookReviews.Commands.CreateBookReview;

public class CreateBookReviewCommandHandler : IRequestHandler<CreateBookReviewCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly IPublisher _publisher;
    private readonly IPointsService _pointsService;
    private readonly ICurrentUserService _currentUserService;

    public CreateBookReviewCommandHandler(IAppDbContext context, 
        IPublisher publisher,
        IPointsService pointsService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _publisher = publisher;
        _pointsService = pointsService;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateBookReviewCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Comment))
            throw new BadRequestException("Rəy boş ola bilməz.");

        if (request.Comment.Length > 1000)
            throw new BadRequestException("Rəy maksimum 1000 simvol ola bilər.");

        if (request.Rating < 1 || request.Rating > 5)
            throw new BadRequestException("Reytinq 1 ilə 5 arasında olmalıdır.");

        var bookExists = await _context.Books.AnyAsync(
            x => x.Id == request.BookId && !x.IsDeleted, cancellationToken);

        if (!bookExists)
            throw new NotFoundException("Kitab tapılmadı.");

        var alreadyReviewed = await _context.BookReviews.AnyAsync(
            x => x.BookId == request.BookId && x.UserId == request.UserId,
            cancellationToken);

        if (alreadyReviewed)
            throw new ConflictException("Siz artıq bu kitaba rəy bildirmisiniz.");

        var review = new BookReview
        {
            BookId = request.BookId,
            UserId = request.UserId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        await _context.BookReviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new BookRatingChangedEvent(review.BookId), cancellationToken);
        await _pointsService.AwardPointsAsync(_currentUserService.UserId, PointAction.AddReview, cancellationToken);
        return review.Id;
    }
}