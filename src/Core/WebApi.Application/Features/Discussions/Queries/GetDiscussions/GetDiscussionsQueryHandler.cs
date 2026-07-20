using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Discussions.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Discussions.Queries.GetDiscussions;

public class GetDiscussionsQueryHandler : IRequestHandler<GetDiscussionsQuery, List<DiscussionDto>>
{
    private readonly IAppDbContext _context;

    public GetDiscussionsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DiscussionDto>> Handle(GetDiscussionsQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize > 100 ? 100 : (request.PageSize < 1 ? 20 : request.PageSize);
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;

        var query = _context.Discussions.AsQueryable();

        if (request.Category.HasValue)
            query = query.Where(d => d.Category == request.Category.Value);

        return await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DiscussionDto
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
                CreatedAt = d.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}