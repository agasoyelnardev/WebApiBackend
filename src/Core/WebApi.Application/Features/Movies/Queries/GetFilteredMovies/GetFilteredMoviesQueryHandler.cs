using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Movies.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Movies.Queries.GetFilteredMovies;

public class GetFilteredMoviesQueryHandler : IRequestHandler<GetFilteredMoviesQuery, List<MovieDto>>
{
    private readonly IAppDbContext _context;

    public GetFilteredMoviesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovieDto>> Handle(GetFilteredMoviesQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize > 100 ? 100 : (request.PageSize < 1 ? 20 : request.PageSize);
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;

        IQueryable<Movie> query = _context.Movies
            .Where(x => !x.IsDeleted);

        // Axtarış sözü filtri (Film adı və ya Rejissor)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            string term = request.SearchTerm.Trim().ToLower();
            query = query.Where(m =>
                EF.Functions.Like(m.Title.ToLower(), $"%{term}%") ||
                EF.Functions.Like(m.Director.ToLower(), $"%{term}%"));
        }

        // Janr filtri
        if (!string.IsNullOrWhiteSpace(request.Genre) && request.Genre != "Hamsı")
        {
            query = query.Where(m => m.Genres.Contains(request.Genre));
        }

        // İl (Tarix) filtri — aralıq əsaslı
        if (!string.IsNullOrWhiteSpace(request.YearFilter) && request.YearFilter != "Hamsı")
        {
            query = request.YearFilter switch
            {
                "2020+" => query.Where(m => m.Year >= 2020),
                "2010s" => query.Where(m => m.Year >= 2010 && m.Year <= 2019),
                "2000s" => query.Where(m => m.Year >= 2000 && m.Year <= 2009),
                "Köhnə" => query.Where(m => m.Year < 2000),
                _ => query
            };
        }

        // Reytinq filtri
        if (!string.IsNullOrWhiteSpace(request.RatingFilter) && request.RatingFilter != "Hamsı")
        {
            if (double.TryParse(request.RatingFilter, out var minRating))
            {
                query = query.Where(m => m.Rating >= minRating);
            }
        }

        if (request.IsTrending.HasValue)
            query = query.Where(m => m.IsTrending == request.IsTrending);

        if (request.IsTopRated.HasValue)
            query = query.Where(m => m.IsTopRated == request.IsTopRated);

        if (request.IsNewRelease.HasValue)
            query = query.Where(m => m.IsNewRelease == request.IsNewRelease);

        // Sıralama məntiqi
        query = request.SortBy switch
        {
            "rating-desc" => query.OrderByDescending(m => m.Rating),
            "year-desc" => query.OrderByDescending(m => m.Year),
            "likes-desc" => query.OrderByDescending(m => m.Likes),
            _ => query.OrderByDescending(m => m.CreatedAt)
        };

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                OriginalTitle = m.OriginalTitle,
                Description = m.Description,
                Poster = m.Poster,
                Banner = m.Banner,
                Rating = m.Rating,
                Year = m.Year,
                Duration = m.Duration,
                Director = m.Director,
                TrailerUrl = m.TrailerUrl,
                VideoUrl = m.VideoUrl,
                Genres = m.Genres,
                Cast = m.Cast,
                Likes = m.Likes,
                ExternalUrl = m.ExternalUrl,
            })
            .ToListAsync(cancellationToken);
    }
}