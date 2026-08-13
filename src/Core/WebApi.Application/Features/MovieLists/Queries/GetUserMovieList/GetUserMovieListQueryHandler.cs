using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Movies.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.MovieLists.Queries.GetUserMovieList;

public class GetUserMovieListQueryHandler
    : IRequestHandler<GetUserMovieListQuery, List<MovieDto>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetUserMovieListQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<MovieDto>> Handle(GetUserMovieListQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        return await _context.UserMovieLists
            .Where(x => x.UserId == currentUserId && x.Type == request.Type && !x.Movie.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MovieDto
            {
                Id = x.Movie.Id,
                Title = x.Movie.Title,
                OriginalTitle = x.Movie.OriginalTitle,
                Description = x.Movie.Description,
                Poster = x.Movie.Poster,
                Banner = x.Movie.Banner,
                Rating = x.Movie.Rating,
                Year = x.Movie.Year,
                Duration = x.Movie.Duration,
                Director = x.Movie.Director,
                TrailerUrl = x.Movie.TrailerUrl,
                VideoUrl = x.Movie.VideoUrl,
                Genres = x.Movie.Genres,
                Cast = x.Movie.Cast
            })
            .ToListAsync(cancellationToken);
    }
}