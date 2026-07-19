using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.BookVsMovies.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookVsMovies.Queries.GetBookVsMovieById;

public class GetBookVsMovieByIdQueryHandler
    : IRequestHandler<GetBookVsMovieByIdQuery, BookVsMovieDto?>
{
    private readonly IAppDbContext _context;

    public GetBookVsMovieByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<BookVsMovieDto?> Handle(
        GetBookVsMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var comparison = await _context.BookVsMovies
            .Where(x => x.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (comparison is null)
            return null;

        if (!string.IsNullOrEmpty(request.RequestingUserId))
        {
            var myVote = await _context.BookVsMovieVotes
                .Where(v => v.BookVsMovieId == request.Id && v.UserId == request.RequestingUserId)
                .Select(v => (VoteChoice?)v.Choice)
                .FirstOrDefaultAsync(cancellationToken);

            comparison.MyVote = myVote?.ToString();
        }

        return comparison;
    }
}