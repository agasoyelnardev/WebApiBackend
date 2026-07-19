using MediatR;
using WebApi.Application.Features.Books.Dtos;

namespace WebApi.Application.Features.Books.Queries.GetFilteredBooks;

public class GetFilteredBooksQuery : IRequest<List<BookDto>>
{
    public string? SearchTerm { get; set; }
    public string? Language { get; set; }
    public int? Year { get; set; }
    public double? MinRating { get; set; }
    public bool? IsTrending { get; set; }
    public bool? IsTopRated { get; set; }
    public bool? IsNewRelease { get; set; }
    public string? SortBy { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}