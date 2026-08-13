using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.BookVsMovies.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookVsMovies.Queries.GetAllBookVsMovies;

public class GetAllBookVsMoviesQueryHandler
    : IRequestHandler<GetAllBookVsMoviesQuery, List<BookVsMovieDto>>
{
    private readonly IAppDbContext _context;

    public GetAllBookVsMoviesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookVsMovieDto>> Handle(
        GetAllBookVsMoviesQuery request, CancellationToken cancellationToken)
    {
        return await _context.BookVsMovies
            .Select(x => new BookVsMovieDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                BookId = x.BookId,
                BookTitle = x.Book.Title,
                BookCover = x.Book.Cover,
                MovieId = x.MovieId,
                MovieTitle = x.Movie.Title,
                MoviePoster = x.Movie.Poster,
                BookVotes = x.BookVotes,
                MovieVotes = x.MovieVotes
            })
            .OrderByDescending(x => x.BookVotes + x.MovieVotes)
            .ToListAsync(cancellationToken);
    }
}