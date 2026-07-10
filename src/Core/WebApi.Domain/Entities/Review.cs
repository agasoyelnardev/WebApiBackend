using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class Review : BaseEntity
{
    public string Content { get; set; } = string.Empty;

    public double Rating { get; set; }

    public Guid MovieId { get; set; }

    public Movie Movie { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public AppUser User { get; set; } = null!;
}