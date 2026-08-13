using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class SavedMovieCollection : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public Guid MovieCollectionId { get; set; }
    public virtual MovieCollection MovieCollection { get; set; } = null!;
}