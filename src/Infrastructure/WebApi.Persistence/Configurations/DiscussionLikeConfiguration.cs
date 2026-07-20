using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Infrastructure.Persistence.Configurations;

public class DiscussionLikeConfiguration : IEntityTypeConfiguration<DiscussionLike>
{
    public void Configure(EntityTypeBuilder<DiscussionLike> builder)
    {
        builder.HasKey(dl => dl.Id);

        // Bir istifadəçi bir müzakirəni yalnız 1 dəfə bəyənə bilsin! (Composite Unique Index)
        builder.HasIndex(dl => new { dl.DiscussionId, dl.UserId })
            .IsUnique();

        // Müzakirə silindikdə bəyənmələr avtomatik silinsin (Cascade)
        builder.HasOne(dl => dl.Discussion)
            .WithMany(d => d.Likes)
            .HasForeignKey(dl => dl.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);

        // İstifadəçi silindikdə Restrict/NoAction edirik (Cascade xətalarının qarşısını almaq üçün)
        builder.HasOne(dl => dl.User)
            .WithMany()
            .HasForeignKey(dl => dl.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}