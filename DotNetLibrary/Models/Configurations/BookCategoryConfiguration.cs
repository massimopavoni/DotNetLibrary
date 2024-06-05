using DotNetLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetLibrary.Models.Configurations;

public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.ToTable("BookCategories");

        builder.HasKey(bc => new { bc.BookISBN, bc.CategoryName });

        builder.HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookISBN)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(bc => bc.Category)
            .WithMany(c => c.CategoryBooks)
            .HasForeignKey(bc => bc.CategoryName)
            .OnDelete(DeleteBehavior.Cascade);
    }
}