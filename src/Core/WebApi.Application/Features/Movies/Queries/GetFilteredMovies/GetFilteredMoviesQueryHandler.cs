using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Movies.Queries.GetMovieById;
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
                EF.Functions.Like(m.Title, $"%{term}%") ||
                EF.Functions.Like(m.Director, $"%{term}%"));
        }

        // Janr filtri: Siyahının daxilində axtarış edir
        if (!string.IsNullOrWhiteSpace(request.Genre) && request.Genre != "Bütün Janrlar")
        {
            query = query.Where(m => m.Genres.Contains(request.Genre));
        }

        // İl filtri
        if (request.Year.HasValue && request.Year > 0)
        {
            query = query.Where(m => m.Year == request.Year);
        }

        // Reytinq filtri
        if (request.MinRating.HasValue)
        {
            query = query.Where(m => m.Rating >= request.MinRating);
        }

        // Sıralama məntiqi
        query = request.SortBy switch
        {
            "rating_desc" => query.OrderByDescending(m => m.Rating),
            "year_desc" => query.OrderByDescending(m => m.Year),
            "title_asc" => query.OrderBy(m => m.Title),
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
                Likes = m.Likes
            })
            .ToListAsync(cancellationToken);
    }
}
