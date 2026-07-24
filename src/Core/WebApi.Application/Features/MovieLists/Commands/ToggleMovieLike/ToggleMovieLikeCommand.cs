using MediatR;

namespace WebApi.Application.Features.MovieLists.Commands.ToggleMovieLike;

public class ToggleMovieLikeCommand : IRequest<bool>
{
    public Guid MovieId { get; set; }
    public string UserId { get; set; } = string.Empty;
}