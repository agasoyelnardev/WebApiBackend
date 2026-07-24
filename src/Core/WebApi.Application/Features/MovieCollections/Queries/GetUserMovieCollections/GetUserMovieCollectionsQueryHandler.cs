using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.MovieCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieCollections.Queries.GetUserMovieCollections;

public class GetUserMovieCollectionsQueryHandler
    : IRequestHandler<GetUserMovieCollectionsQuery, List<MovieCollectionDto>>
{
    private readonly IAppDbContext _context;

    public GetUserMovieCollectionsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovieCollectionDto>> Handle(
        GetUserMovieCollectionsQuery request, CancellationToken cancellationToken)
    {
        var isOwner = !string.IsNullOrEmpty(request.RequestingUserId)
                      && request.RequestingUserId == request.TargetUserId;

        var query = _context.MovieCollections
            .Where(c => c.AppUserId == request.TargetUserId);

        // Sahibi deyilsə, yalnız ictimai kolleksiyaları göstər
        if (!isOwner)
            query = query.Where(c => c.IsPublic);

        return await query
            .Select(c => new MovieCollectionDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CoverImageUrl = c.CoverImageUrl,
                IsPublic = c.IsPublic,
                AppUserId = c.AppUserId,
                MovieCount = c.Items.Count,
                LikesCount = c.Likes.Count,
                IsSaved = !isOwner && (!string.IsNullOrEmpty(request.RequestingUserId) && 
                                       _context.SavedMovieCollections.Any(
                                           s => s.MovieCollectionId == c.Id && s.UserId == request.RequestingUserId)),
                IsLikedByCurrentUser = !string.IsNullOrEmpty(request.RequestingUserId) &&
                                       c.Likes.Any(l => l.UserId == request.RequestingUserId)
            })
            .ToListAsync(cancellationToken);
    }
}