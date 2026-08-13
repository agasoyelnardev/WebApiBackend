using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class MovieCollectionItem : BaseEntity
{
    // Hansı kolleksiyaya aid olduğu
    public Guid MovieCollectionId { get; set; }
    public MovieCollection MovieCollection { get; set; } = null!;

    // Hansı filmə aid olduğu
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    // Filmin kolleksiyaya əlavə edilmə tarixi
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}