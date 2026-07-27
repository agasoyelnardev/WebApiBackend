using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Author)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(4000);

        builder.Property(x => x.Cover)
            .HasMaxLength(500);

        builder.Property(x => x.Language)
            .HasMaxLength(10)
            .HasDefaultValue("az");

        builder.Property(x => x.Rating)
            .HasPrecision(3, 2); // 0.00 - 9.99

        builder.Property(x => x.DownloadUrl)
            .HasMaxLength(500);

        builder.Property(x => x.PdfUrl)
            .HasMaxLength(500);

        builder.Property(x => x.CustomContent)
            .HasColumnType("nvarchar(max)"); // MS SQL Server üçün (Postgres üçün text)

        builder.Property(x => x.IsTrending)
            .HasDefaultValue(false);

        builder.Property(x => x.IsTopRated)
            .HasDefaultValue(false);

        builder.Property(x => x.IsNewRelease)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()") // DB səviyyəsində də tarix təyin edir
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        // Global Query Filter (Soft Delete)
        builder.HasQueryFilter(x => !x.IsDeleted);

        // Indexes
        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.Author);
        builder.HasIndex(x => new { x.IsTrending, x.IsDeleted });
        builder.HasIndex(x => new { x.IsTopRated, x.IsDeleted });
        builder.HasIndex(x => new { x.IsNewRelease, x.IsDeleted });

        // Relationships

        // Book (1) -> BookReviews (Many)
        builder.HasMany(x => x.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Book (1) -> CollectionItems (Many)
        builder.HasMany(x => x.CollectionItems)
            .WithOne(ci => ci.Book)
            .HasForeignKey(ci => ci.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Book (1) -> MovieAdaptations (Many)
        builder.HasMany(x => x.MovieAdaptations)
            .WithOne(m => m.BookSource)
            .HasForeignKey(m => m.BookSourceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}