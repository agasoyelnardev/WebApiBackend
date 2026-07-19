using MediatR;

namespace WebApi.Application.Features.BookVsMovies.Commands.DeleteBookVsMovie;

public record DeleteBookVsMovieCommand(Guid Id) : IRequest;