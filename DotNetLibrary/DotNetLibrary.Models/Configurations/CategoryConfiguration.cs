using DotNetLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetLibrary.Models.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public const int NameMaxLength = 256;
    public const int DescriptionMaxLength = 65535;

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Name);

        builder.Property(c => c.Name)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(c => c.Description)
            .HasMaxLength(65535);
    }
}