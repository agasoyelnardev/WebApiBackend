using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Infrastructure.Persistence.Configurations;

public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Content)
            .IsRequired()
            .HasMaxLength(5000);

        // Enum dəyərini bazada rəqəm deyil, oxunaqlı string (məs. "Reviews") kimi saxlamaq üçün:
        builder.Property(d => d.Category)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        // Author (AppUser) ilə əlaqə
        builder.HasOne(d => d.Author)
            .WithMany() 
            .HasForeignKey(d => d.AuthorId)
            .OnDelete(DeleteBehavior.Restrict); // İstifadəçi silindikdə onun bütün postları birdən-birə silinməsin (Restrict)
    }
}