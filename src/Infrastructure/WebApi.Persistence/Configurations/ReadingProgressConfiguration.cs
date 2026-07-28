using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class ReadingProgressConfiguration : IEntityTypeConfiguration<ReadingProgress>
{
    public void Configure(EntityTypeBuilder<ReadingProgress> builder)
    {
        
        builder.HasIndex(x => new { x.UserId, x.BookId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.ReadingProgresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Book)
            .WithMany()
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}