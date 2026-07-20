using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(1000);

        // Discussion silindikdə şərhlər avtomatik silinsin (Cascade)
        builder.HasOne(c => c.Discussion)
            .WithMany(d => d.Comments)
            .HasForeignKey(c => c.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Şərh yazan istifadəçi silindikdə xəta çıxmasın deyə Restrict edilir
        builder.HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}