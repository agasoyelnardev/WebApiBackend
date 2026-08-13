using MediatR;
using WebApi.Application.Features.MovieCollections.Dtos;

namespace WebApi.Application.Features.MovieCollections.Queries.GetSavedMovieCollections;

public class GetSavedMovieCollectionsQuery : IRequest<List<MovieCollectionDto>>
{
}