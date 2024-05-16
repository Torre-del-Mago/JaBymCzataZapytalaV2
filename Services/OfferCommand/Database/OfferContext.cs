using Microsoft.EntityFrameworkCore;
using OfferCommand.Database.Tables;
using OfferQuery.Database.Entity;

namespace OfferCommand.Database;

public class OfferContext : DbContext
{

    public DbSet<Offer> Offers { get; set; }

    public DbSet<OfferEvent> ReservedRooms { get; set; }

    public DbSet<OfferRoom> OfferRooms { get; set; }

    private readonly IConfiguration configuration;
    public OfferContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbPath"));

}