using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.FullName)
            .HasMaxLength(100);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Avatar)
            .HasMaxLength(500);

        builder.Property(x => x.Bio)
            .HasMaxLength(500);

        builder.Property(x => x.LastPremiumPlan)
            .HasMaxLength(20);

        
        builder.Ignore(x => x.IsPremium);
    }
}