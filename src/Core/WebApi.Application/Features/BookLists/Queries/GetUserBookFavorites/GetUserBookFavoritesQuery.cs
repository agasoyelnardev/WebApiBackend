using MediatR;
using WebApi.Application.Features.Books.Dtos;

namespace WebApi.Application.Features.BookLists.Queries.GetUserBookFavorites;

public record GetUserBookFavoritesQuery(string UserId) : IRequest<List<BookDto>>;