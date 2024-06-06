using DotNetLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetLibrary.Models.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.EmailAddress);

        builder.Property(u => u.EmailAddress)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(u => u.PasswordHash)
            .HasMaxLength(60)
            .IsRequired();
        builder.Property(u => u.Role)
            .IsRequired();
        builder.Property(u => u.FirstName)
            .HasMaxLength(256);
        builder.Property(u => u.LastName)
            .HasMaxLength(256);
    }
}