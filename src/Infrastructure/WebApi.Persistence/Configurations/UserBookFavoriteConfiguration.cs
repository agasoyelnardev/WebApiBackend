using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class UserBookFavoriteConfiguration : IEntityTypeConfiguration<UserBookFavorite>
{
    public void Configure(EntityTypeBuilder<UserBookFavorite> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.BookId })
            .IsUnique();
    }
}