using Accidents.Model;

using Komunalka.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;

using Protocols.Model;

namespace Komunalka.Persistence;

public class KomunalkaContext : DbContext
{
    public KomunalkaContext(DbContextOptions<KomunalkaContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Accident> Accidents { get; set; }

    public DbSet<Protocol> Protocols { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("komunalka");

        modelBuilder.ApplyConfiguration(new ProtocolConfiguration());
        modelBuilder.ApplyConfiguration(new AccidentConfiguration());
    }
}