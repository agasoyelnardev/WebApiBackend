using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Author)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Rating)
            .IsRequired();

        // One-to-Many əlaqəsi (Bir filmin çoxlu rəyi, hər rəyin isə yalnız bir filmi ola bilər)
        builder.HasOne(r => r.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade); // Əgər film silinərsə, ona yazılan rəylər də avtomatik silinsin
    }
}