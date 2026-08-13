using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public enum MovieListType
{
    Favorite,
    Watchlist
}

public class UserMovieList : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public Guid MovieId { get; set; }
    public virtual Movie Movie { get; set; } = null!;

    public MovieListType Type { get; set; }
}