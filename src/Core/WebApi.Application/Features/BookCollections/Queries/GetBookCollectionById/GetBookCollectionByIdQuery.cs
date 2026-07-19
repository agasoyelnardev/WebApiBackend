using MediatR;
using WebApi.Application.Features.BookCollections.Dtos;

namespace WebApi.Application.Features.BookCollections.Queries.GetBookCollectionById;

public record GetBookCollectionByIdQuery(Guid Id) : IRequest<BookCollectionDetailDto?>;