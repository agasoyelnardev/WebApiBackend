using MediatR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Queries.SearchGoogleBooks;

public record SearchGoogleBooksQuery(string Query) : IRequest<List<GoogleBooksSearchResultDto>>;