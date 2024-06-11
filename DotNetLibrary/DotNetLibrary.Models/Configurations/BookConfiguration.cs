using DotNetLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetLibrary.Models.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public const int ISBNMaxLength = 17;
    public const int TitleMaxLength = 65535;
    public const int AuthorMaxLength = 512;
    public const int PublicationDateMaxFutureYears = 2;
    public const int PublisherMaxLength = 512;
    public const int MinCategories = 1;
    public const int MaxCategories = 16;

    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.ISBN);

        builder.Property(b => b.ISBN)
            .HasMaxLength(17)
            .IsRequired();
        builder.Property(b => b.Title)
            .HasMaxLength(65535)
            .IsRequired();
        builder.Property(b => b.Author)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(b => b.PublicationDate)
            .IsRequired();
        builder.Property(b => b.Publisher)
            .HasMaxLength(512)
            .IsRequired();
    }
}