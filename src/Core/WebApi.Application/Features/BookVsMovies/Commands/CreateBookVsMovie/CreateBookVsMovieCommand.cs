using MediatR;

namespace WebApi.Application.Features.BookVsMovies.Commands.CreateBookVsMovie;

public class CreateBookVsMovieCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid BookId { get; set; }
    public Guid MovieId { get; set; }
}