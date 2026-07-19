using MediatR;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookVsMovies.Commands.Vote;

public class VoteCommand : IRequest
{
    public Guid BookVsMovieId { get; set; }
    public VoteChoice Choice { get; set; }

    public string UserId { get; set; } = string.Empty;
}