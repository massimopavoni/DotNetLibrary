using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.ID);

        builder.Property(c => c.ID)
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(c => c.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}