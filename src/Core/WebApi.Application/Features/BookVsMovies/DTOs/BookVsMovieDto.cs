namespace WebApi.Application.Features.BookVsMovies.Dtos;

public class BookVsMovieDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookCover { get; set; } = string.Empty;

    public Guid MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string MoviePoster { get; set; } = string.Empty;

    public int BookVotes { get; set; }
    public int MovieVotes { get; set; }

    // Sorğunu göndərən istifadəçinin öz səsi (varsa)
    public string? MyVote { get; set; } // "Book", "Movie" və ya null
}