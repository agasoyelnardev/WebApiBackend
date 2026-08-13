using MediatR;
using WebApi.Application.Features.BookVsMovies.Dtos;

namespace WebApi.Application.Features.BookVsMovies.Queries.GetAllBookVsMovies;

public record GetAllBookVsMoviesQuery : IRequest<List<BookVsMovieDto>>;