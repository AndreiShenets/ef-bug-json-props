using Microsoft.EntityFrameworkCore;

namespace EfCoreException;

public sealed class DatabaseContext : DbContext
{
    public DbSet<Entity> Entities { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>(
            entity =>
            {
                entity.ToTable("Entities");
                entity.HasKey(e => e.Id);

                entity.OwnsMany(
                    e => e.L1OwnedEntities,
                    ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder.ToJson();

                        ownedNavigationBuilder.OwnsMany(
                            e => e.L2OwnedEntities,
                            foundValuesOwnedNavigationBuilder =>
                            {
                                foundValuesOwnedNavigationBuilder.ToJson();

                                foundValuesOwnedNavigationBuilder.OwnsMany(
                                    e => e.Values,
                                    valuesOwnedNavigationBuilder =>
                                    {
                                        valuesOwnedNavigationBuilder.ToJson();
                                    }
                                );
                            }
                        );
                    }
                );
            }
        );
    }
}