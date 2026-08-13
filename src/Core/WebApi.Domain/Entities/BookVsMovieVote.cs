using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public enum VoteChoice
{
    Book,
    Movie
}

public class BookVsMovieVote : BaseEntity
{
    public Guid BookVsMovieId { get; set; }
    public virtual BookVsMovie BookVsMovie { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public VoteChoice Choice { get; set; }
}