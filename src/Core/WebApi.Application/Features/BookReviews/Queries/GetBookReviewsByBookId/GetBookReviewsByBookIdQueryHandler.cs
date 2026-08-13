using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.BookReviews.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookReviews.Queries.GetBookReviewsByBookId;

public class GetBookReviewsByBookIdQueryHandler
    : IRequestHandler<GetBookReviewsByBookIdQuery, List<BookReviewDto>>
{
    private readonly IAppDbContext _context;

    public GetBookReviewsByBookIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookReviewDto>> Handle(
        GetBookReviewsByBookIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.BookReviews
            .Where(r => r.BookId == request.BookId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new BookReviewDto
            {
                Id = r.Id,
                Author = r.User.UserName ?? "Unknown",
                UserAvatar = r.User.Avatar,
                Rating = r.Rating,
                Comment = r.Comment,
                Likes = r.Likes,
                Dislikes = r.Dislikes,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}