using MediatR;

namespace WebApi.Application.Features.Movies.Commands.CreateMovie;

public class CreateMovieCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public string Banner { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int Year { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string TrailerUrl { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }

    public List<string> Genres { get; set; } = new();
    public List<string> Cast { get; set; } = new();
}