using MediatR;

namespace WebApi.Application.Features.Movies.Commands.ImportMovieFromTmdb;

public class ImportMovieFromTmdbCommand : IRequest<Guid>
{
    public int TmdbId { get; set; }
}