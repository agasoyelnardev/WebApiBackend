using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class UserMovieListConfiguration : IEntityTypeConfiguration<UserMovieList>
{
    public void Configure(EntityTypeBuilder<UserMovieList> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.MovieId, x.Type })
            .IsUnique();
    }
}