using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.MovieCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieCollections.Queries.GetSavedMovieCollections;

public class GetSavedMovieCollectionsQueryHandler
    : IRequestHandler<GetSavedMovieCollectionsQuery, List<MovieCollectionDto>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSavedMovieCollectionsQueryHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<MovieCollectionDto>> Handle(
        GetSavedMovieCollectionsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");
        
            
        return await _context.SavedMovieCollections
            .Where(x => x.UserId == currentUserId && x.MovieCollection.IsPublic)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MovieCollectionDto
            {
                Id = x.MovieCollection.Id,
                Name = x.MovieCollection.Name,
                Description = x.MovieCollection.Description,
                CoverImageUrl = x.MovieCollection.CoverImageUrl,
                IsPublic = x.MovieCollection.IsPublic,
                AppUserId = x.MovieCollection.AppUserId,
                MovieCount = x.MovieCollection.Items.Count
            })
            .ToListAsync(cancellationToken);
    }
}