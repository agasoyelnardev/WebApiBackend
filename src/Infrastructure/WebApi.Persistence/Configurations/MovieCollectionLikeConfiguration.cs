using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class MovieCollectionLikeConfiguration
    : IEntityTypeConfiguration<MovieCollectionLike>
{
    public void Configure(EntityTypeBuilder<MovieCollectionLike> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.MovieCollectionId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.MovieCollection)
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.MovieCollectionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}