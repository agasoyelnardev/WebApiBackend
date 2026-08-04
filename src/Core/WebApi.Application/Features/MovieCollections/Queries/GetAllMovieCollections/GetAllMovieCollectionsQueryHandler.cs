using MediatR;
using WebApi.Application.Common.Models;
using WebApi.Application.Features.MovieCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieCollections.Queries.GetAllMovieCollections;

public class GetAllMovieCollectionsQueryHandler
    : IRequestHandler<GetAllMovieCollectionsQuery, PaginatedList<MovieCollectionListItemDto>>
{
    private readonly IAppDbContext _context;

    public GetAllMovieCollectionsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MovieCollectionListItemDto>> Handle(
        GetAllMovieCollectionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MovieCollections
            .Where(c => c.IsPublic)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new MovieCollectionListItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CoverImageUrl = c.CoverImageUrl,
                AppUserId = c.AppUserId,
                Username = c.AppUser.UserName ?? string.Empty,
                MovieCount = c.Items.Count,
                LikesCount = c.Likes.Count,
                CreatedAt = c.CreatedAt
            });

        return await PaginatedList<MovieCollectionListItemDto>.CreateAsync(query, request.Page, request.PageSize);
    }
}