using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
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