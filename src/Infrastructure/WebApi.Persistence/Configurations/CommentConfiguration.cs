using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);   


    }
}