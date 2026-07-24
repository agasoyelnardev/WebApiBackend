using MediatR;
using WebApi.Application.Features.Books.Dtos;

namespace WebApi.Application.Features.BookLists.Queries.GetUserBookWatchlist;

public record GetUserBookWatchlistQuery(string UserId) : IRequest<List<BookDto>>;