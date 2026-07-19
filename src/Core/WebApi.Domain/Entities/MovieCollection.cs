using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class MovieCollection : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublic { get; set; } = true;

    public string AppUserId { get; set; } = string.Empty;
    public AppUser AppUser { get; set; } = null!;

    public ICollection<MovieCollectionItem> Items { get; set; } = new List<MovieCollectionItem>();
}