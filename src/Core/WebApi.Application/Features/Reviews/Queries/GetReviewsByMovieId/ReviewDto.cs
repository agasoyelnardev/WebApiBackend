namespace WebApi.Application.Features.Reviews.Queries.GetReviewsByMovieId;

public class ReviewDto
{
    public Guid Id { get; set; }

    public string Author { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public double Rating { get; set; }
}