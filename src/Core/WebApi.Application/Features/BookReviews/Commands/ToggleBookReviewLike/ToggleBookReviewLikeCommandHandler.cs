using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.BookReviews.Commands.ToggleBookReviewLike;

public class ToggleBookReviewLikeCommandHandler : IRequestHandler<ToggleBookReviewLikeCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleBookReviewLikeCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleBookReviewLikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var review = await _context.BookReviews
            .FirstOrDefaultAsync(r => r.Id == request.BookReviewId, cancellationToken);

        if (review is null)
            throw new NotFoundException("Rəy tapılmadı.");

        var existing = await _context.BookReviewLikes.FirstOrDefaultAsync(
            x => x.UserId == request.UserId && x.BookReviewId == request.BookReviewId,
            cancellationToken);

        if (existing is null)
        {
            var like = new BookReviewLike
            {
                UserId = request.UserId,
                BookReviewId = request.BookReviewId,
                Choice = request.Choice
            };

            await _context.BookReviewLikes.AddAsync(like, cancellationToken);

            if (request.Choice == ReviewLikeChoice.Like)
                review.Likes++;
            else
                review.Dislikes++;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        if (existing.Choice == request.Choice)
        {
            _context.BookReviewLikes.Remove(existing);

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