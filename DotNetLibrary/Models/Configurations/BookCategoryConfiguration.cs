using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations;

public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.ToTable("BookCategories");

        builder.HasKey(bc => new { bc.BookISBN, bc.CategoryID });

        builder.HasOne(bc => bc.Book)
            .WithMany(b => b.Categories)
            .HasForeignKey(bc => bc.BookISBN)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(bc => bc.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(bc => bc.CategoryID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}