using MediatR;

namespace WebApi.Application.Features.MovieLists.Commands.ToggleFavorite;

public class ToggleFavoriteCommand : IRequest<bool>
{
    public Guid MovieId { get; set; }
    public string UserId { get; set; } = string.Empty;
}