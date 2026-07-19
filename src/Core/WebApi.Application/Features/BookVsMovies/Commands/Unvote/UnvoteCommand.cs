using MediatR;

namespace WebApi.Application.Features.BookVsMovies.Commands.Unvote;

public class UnvoteCommand : IRequest
{
    public Guid BookVsMovieId { get; set; }
    public string UserId { get; set; } = string.Empty;
}