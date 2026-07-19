using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class BookVsMovie : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public Guid BookId { get; set; }
    public virtual Book Book { get; set; } = null!;

    public Guid MovieId { get; set; }
    public virtual Movie Movie { get; set; } = null!;

    public int BookVotes { get; set; }
    public int MovieVotes { get; set; }
    
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<BookVsMovieVote> Votes { get; set; }
        = new List<BookVsMovieVote>();
}