using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

public class BookVsMovieVoteConfiguration
    : IEntityTypeConfiguration<BookVsMovieVote>
{
    public void Configure(EntityTypeBuilder<BookVsMovieVote> builder)
    {
        builder.HasOne(x => x.BookVsMovie)
            .WithMany(x => x.Votes)
            .HasForeignKey(x => x.BookVsMovieId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.BookVsMovieVotes)
            .HasForeignKey(x => x.UserId);

        builder.HasIndex(x => new
        {
            x.BookVsMovieId,
            x.UserId
        }).IsUnique();
    }
}