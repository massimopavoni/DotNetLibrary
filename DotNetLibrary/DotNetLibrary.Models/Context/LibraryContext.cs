using DotNetLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetLibrary.Models.Context;

public class LibraryContext(DbContextOptions<LibraryContext> config) : DbContext(config)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(builder =>
            builder.EnableRetryOnFailure(8, TimeSpan.FromSeconds(16), null)
        );
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}