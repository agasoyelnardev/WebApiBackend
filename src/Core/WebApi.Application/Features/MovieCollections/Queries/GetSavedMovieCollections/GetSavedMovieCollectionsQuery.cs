using MediatR;
using WebApi.Application.Features.MovieCollections.Dtos;

namespace WebApi.Application.Features.MovieCollections.Queries.GetSavedMovieCollections;

public record GetSavedMovieCollectionsQuery(string UserId) : IRequest<List<MovieCollectionDto>>;