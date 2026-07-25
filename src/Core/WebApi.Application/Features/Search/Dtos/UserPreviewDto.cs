using WebApi.Application.Features.Books.Dtos;
using WebApi.Application.Features.BookCollections.Dtos;
using WebApi.Application.Features.Discussions.Dtos;
using WebApi.Application.Features.MovieCollections.Dtos;
using WebApi.Application.Features.Movies.Dtos;
using WebApi.Application.Features.Movies.Queries.GetMovieById;

namespace WebApi.Application.Features.Search.Dtos;

public class UserPreviewDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
}

public class GlobalSearchResultDto
{
    public List<MovieDto> Movies { get; set; } = new();
    public List<BookDto> Books { get; set; } = new();
    public List<UserPreviewDto> Users { get; set; } = new();
    public List<MovieCollectionDto> MovieCollections { get; set; } = new();
    public List<BookCollectionDto> BookCollections { get; set; } = new();
    public List<DiscussionDto> Discussions { get; set; } = new();
}