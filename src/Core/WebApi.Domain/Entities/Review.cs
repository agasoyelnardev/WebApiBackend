using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Base;

public class Review : BaseEntity
{
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public double Rating { get; set; }

    public Guid MovieId { get; set; }

    public Movie Movie { get; set; } = null!;
}