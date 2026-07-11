using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Movies.Queries.GetFilteredMovies;

public class GetFilteredMoviesQueryHandler : IRequestHandler<GetFilteredMoviesQuery, List<Movie>>
{
    private readonly IAppDbContext _context; 

    public GetFilteredMoviesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> Handle(GetFilteredMoviesQuery request, CancellationToken cancellationToken)
    {
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

        // YENİ JANR FİLTRİ: Siyahının daxilində axtarış edir
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
            _ => query.OrderByDescending(m => m.Id) 
        };

        return await query.ToListAsync(cancellationToken);
    }
}