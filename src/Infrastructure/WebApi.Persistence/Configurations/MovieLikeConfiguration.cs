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
    }
}