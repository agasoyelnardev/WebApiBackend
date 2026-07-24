using MediatR;
using WebApi.Application.Features.BookCollections.Dtos;

namespace WebApi.Application.Features.BookCollections.Queries.GetSavedBookCollections;

public record GetSavedBookCollectionsQuery(string UserId) : IRequest<List<BookCollectionDto>>;