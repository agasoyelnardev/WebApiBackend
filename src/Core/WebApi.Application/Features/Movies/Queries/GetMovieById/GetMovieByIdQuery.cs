using MediatR;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Movies.Queries.GetMovieById;

public class GetMovieByIdQuery : IRequest<Movie?>
{
    public Guid Id { get; set; } 

    public GetMovieByIdQuery(Guid id)
    {
        Id = id;
    }
}