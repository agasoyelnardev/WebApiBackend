using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class MovieLikeConfiguration : IEntityTypeConfiguration<MovieLike>
{
    public void Configure(EntityTypeBuilder<MovieLike> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.MovieId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Movie)
            .WithMany()
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}