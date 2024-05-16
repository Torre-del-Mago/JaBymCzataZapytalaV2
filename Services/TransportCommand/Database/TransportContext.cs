using TransportCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace TransportCommand.Database;

public class TransportContext : DbContext
{
    public DbSet<FlightConnection> FlightConnections { get; set; }

    public DbSet<ReservedTicket> ReservedTickets { get; set; }

    public DbSet<Transport> Transports { get; set; }

    private readonly IConfiguration configuration;

    public TransportContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbPath"));

}