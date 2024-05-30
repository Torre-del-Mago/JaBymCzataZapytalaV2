using Microsoft.EntityFrameworkCore;
using OfferCommand.Database.Tables;

namespace OfferCommand.Database;

public class OfferContext : DbContext
{

    public DbSet<Offer> Offers { get; set; }

    public DbSet<OfferEvent> Events { get; set; }

    public DbSet<OfferRoom> Rooms { get; set; }

    public OfferContext(DbContextOptions<OfferContext> options) : base(options) {}
    
    public OfferContext() : base() {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offer>().ToTable("Offer");
        modelBuilder.Entity<OfferEvent>().ToTable("OfferEvent");
        modelBuilder.Entity<OfferRoom>().ToTable("OfferRoom");
    }
}