using MediatR;

namespace WebApi.Application.Features.Movies.Commands.DeleteMovie;

public class DeleteMovieCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public DeleteMovieCommand(Guid id)
    {
        Id = id;
    }
}