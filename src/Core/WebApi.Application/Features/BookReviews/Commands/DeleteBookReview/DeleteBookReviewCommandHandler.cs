using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Events;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookReviews.Commands.DeleteBookReview;

public class DeleteBookReviewCommandHandler : IRequestHandler<DeleteBookReviewCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublisher _publisher;

    public DeleteBookReviewCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IPublisher publisher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _publisher = publisher;
    }

    public async Task Handle(DeleteBookReviewCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var review = await _context.BookReviews
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (review is null)
            throw new NotFoundException("Rəy tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (review.UserId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu rəyi silmək hüququnuz yoxdur.");

        var bookId = review.BookId;

        _context.BookReviews.Remove(review);
        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new BookRatingChangedEvent(bookId), cancellationToken);
    }
}