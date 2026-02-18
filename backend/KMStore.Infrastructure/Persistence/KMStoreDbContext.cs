using KMStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace KMStore.Infrastructure.Persistence;

public class KMStoreDbContext : DbContext
{
    public KMStoreDbContext(DbContextOptions<KMStoreDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryTranslation> CategoryTranslations => Set<CategoryTranslation>();

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductTranslation> ProductTranslations => Set<ProductTranslation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
    new Role { Id = 1, Name = "Admin" },
    new Role { Id = 2, Name = "User" }
);

        // Unique: aynı category için aynı dil 1 kez olsun
        modelBuilder.Entity<CategoryTranslation>()
            .HasIndex(x => new { x.CategoryId, x.LanguageCode })
            .IsUnique();

        // Unique: aynı product için aynı dil 1 kez olsun
        modelBuilder.Entity<ProductTranslation>()
            .HasIndex(x => new { x.ProductId, x.LanguageCode })
            .IsUnique();

        // Email unique olsun
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

    }
}
