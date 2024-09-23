using MemoryPlaces.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemoryPlaces.Infrastructure.Persistance;

public class MemoryPlacesDbContext : DbContext
{
    public MemoryPlacesDbContext(DbContextOptions<MemoryPlacesDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Domain.Entities.Type> Types { get; set; }
    public DbSet<Period> Periods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
