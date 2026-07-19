using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);

        // Genres siyahısını JSON string kimi bazaya yazır və oxuyur
        builder.Property(m => m.Genres)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        // Cast siyahısını JSON string kimi bazaya yazır və oxuyur
        builder.Property(m => m.Cast)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

        // Kitab mənbəyi ilə əlaqə: bir kitabın çoxlu film adaptasiyası ola bilər
        builder.HasOne(m => m.BookSource)
            .WithMany(b => b.MovieAdaptations)
            .HasForeignKey(m => m.BookSourceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}