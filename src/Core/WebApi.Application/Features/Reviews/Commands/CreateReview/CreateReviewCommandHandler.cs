using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Events;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublisher _publisher;

    public CreateReviewCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService,
        IPublisher publisher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 1000)
            throw new BadRequestException("Rəy boş ola bilməz və ya 1000 simvoldan çox ola bilməz.");

        if (request.Rating < 1 || request.Rating > 5)
            throw new BadRequestException("Reytinq 1 ilə 5 arasında olmalıdır.");

        var movieExists = await _context.Movies.AnyAsync(
            x => x.Id == request.MovieId,
            cancellationToken);

        if (!movieExists)
            throw new NotFoundException("Film tapılmadı.");

        var alreadyReviewed = await _context.Reviews.AnyAsync(
            x => x.MovieId == request.MovieId && x.UserId == _currentUserService.UserId,
            cancellationToken);

        if (alreadyReviewed)
            throw new ConflictException("Siz artıq bu filmə rəy bildirmisiniz.");

        var review = new Review
        {
            Content = request.Content,
            Rating = request.Rating,
            MovieId = request.MovieId,
            UserId = _currentUserService.UserId
        };

        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new MovieRatingChangedEvent(review.MovieId), cancellationToken);

        return review.Id;
    }
}