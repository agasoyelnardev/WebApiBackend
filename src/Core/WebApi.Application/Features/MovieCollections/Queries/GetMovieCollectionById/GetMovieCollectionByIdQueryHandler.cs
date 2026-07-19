using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Features.MovieCollections.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieCollections.Queries.GetMovieCollectionById;

public class GetMovieCollectionByIdQueryHandler
    : IRequestHandler<GetMovieCollectionByIdQuery, MovieCollectionDetailDto?>
{
    private readonly IAppDbContext _context;

    public GetMovieCollectionByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<MovieCollectionDetailDto?> Handle(
        GetMovieCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        var collection = await _context.MovieCollections
            .Where(c => c.Id == request.Id)
            .Select(c => new MovieCollectionDetailDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CoverImageUrl = c.CoverImageUrl,
                IsPublic = c.IsPublic,
                AppUserId = c.AppUserId,
                Movies = c.Items
                    .Where(i => !i.Movie.IsDeleted)
                    .Select(i => new MovieSummaryDto
                    {
                        Id = i.Movie.Id,
                        Title = i.Movie.Title,
                        Poster = i.Movie.Poster,
                        Rating = i.Movie.Rating
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (collection is null)
            return null;

        var isOwner = !string.IsNullOrEmpty(request.RequestingUserId)
                      && request.RequestingUserId == collection.AppUserId;

        if (!collection.IsPublic && !isOwner)
            throw new UnauthorizedAccessException("Bu kolleksiya şəxsidir.");

        return collection;
    }
}