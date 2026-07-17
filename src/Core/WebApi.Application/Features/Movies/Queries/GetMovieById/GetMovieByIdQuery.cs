using MediatR;

namespace WebApi.Application.Features.Movies.Queries.GetMovieById;

public class GetMovieByIdQuery : IRequest<MovieDto?>
{
    public Guid Id { get; set; }

    public GetMovieByIdQuery(Guid id)
    {
        Id = id;
    }
}