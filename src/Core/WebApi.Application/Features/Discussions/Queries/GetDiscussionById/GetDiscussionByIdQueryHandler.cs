using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Discussions.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Discussions.Queries.GetDiscussionById;

public class GetDiscussionByIdQueryHandler : IRequestHandler<GetDiscussionByIdQuery, DiscussionDetailDto?>
{
    private readonly IAppDbContext _context;

    public GetDiscussionByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<DiscussionDetailDto?> Handle(GetDiscussionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Discussions
            .Where(d => d.Id == request.Id)
            .Select(d => new DiscussionDetailDto
            {
                Id = d.Id,
                Title = d.Title,
                Content = d.Content,
                Category = d.Category.ToString(),
                AuthorId = d.AuthorId,
                Author = d.Author.UserName ?? "Unknown",
                AuthorAvatar = d.Author.Avatar,
                Likes = d.Likes.Count,
                IsLikedByCurrentUser = request.RequestingUserId != null &&
                                       d.Likes.Any(l => l.UserId == request.RequestingUserId),
                CommentsCount = d.Comments.Count,
                CreatedAt = d.CreatedAt,
                Comments = d.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        AuthorId = c.AuthorId,
                        Author = c.Author.UserName ?? "Unknown",
                        AuthorAvatar = c.Author.Avatar,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}