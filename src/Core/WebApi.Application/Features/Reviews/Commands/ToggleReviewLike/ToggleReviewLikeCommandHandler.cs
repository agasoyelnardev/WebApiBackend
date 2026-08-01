using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Reviews.Commands.ToggleReviewLike;

public class ToggleReviewLikeCommandHandler : IRequestHandler<ToggleReviewLikeCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleReviewLikeCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleReviewLikeCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

        if (review is null)
            throw new NotFoundException("Rəy tapılmadı.");

        var existing = await _context.ReviewLikes.FirstOrDefaultAsync(
            x => x.UserId == currentUserId && x.ReviewId == request.ReviewId,
            cancellationToken);

        if (existing is null)
        {
            var like = new ReviewLike
            {
                UserId = currentUserId,
                ReviewId = request.ReviewId,
                Choice = request.Choice
            };

            await _context.ReviewLikes.AddAsync(like, cancellationToken);

            if (request.Choice == ReviewLikeChoice.Like)
                review.Likes++;
            else
                review.Dislikes++;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        if (existing.Choice == request.Choice)
        {
            _context.ReviewLikes.Remove(existing);

            if (existing.Choice == ReviewLikeChoice.Like)
                review.Likes = Math.Max(0, review.Likes - 1);
            else
                review.Dislikes = Math.Max(0, review.Dislikes - 1);

            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        if (existing.Choice == ReviewLikeChoice.Like)
        {
            review.Likes = Math.Max(0, review.Likes - 1);
            review.Dislikes++;
        }
        else
        {
            review.Dislikes = Math.Max(0, review.Dislikes - 1);
            review.Likes++;
        }

        existing.Choice = request.Choice;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}