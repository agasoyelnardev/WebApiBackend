using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookVsMovieVoteConfiguration : IEntityTypeConfiguration<BookVsMovieVote>
{
    public void Configure(EntityTypeBuilder<BookVsMovieVote> builder)
    {
        // Bir istifadəçi eyni Book vs Movie üçün yalnız 1 dəfə səs verə bilsin
        builder.HasIndex(x => new { x.UserId, x.BookVsMovieId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.BookVsMovieVotes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}