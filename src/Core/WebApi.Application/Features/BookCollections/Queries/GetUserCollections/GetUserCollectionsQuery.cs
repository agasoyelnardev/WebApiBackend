using MediatR;
using WebApi.Application.Features.BookCollections.Dtos;

namespace WebApi.Application.Features.BookCollections.Queries.GetUserCollections;

public record GetUserCollectionsQuery(string UserId) : IRequest<List<BookCollectionDto>>;