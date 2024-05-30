using TransportCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace TransportCommand.Database;

public class TransportContext : DbContext
{
    public DbSet<FlightConnection> FlightConnections { get; set; }

    public DbSet<ReservedTicket> ReservedTickets { get; set; }

    public DbSet<Transport> Transports { get; set; }

    public DbSet<Event> Events { get; set; }

    public TransportContext(DbContextOptions<TransportContext> options) : base(options) {}

    public TransportContext() : base() {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlightConnection>().ToTable("FlightConnection");
        modelBuilder.Entity<ReservedTicket>().ToTable("ReservedTicket");
        modelBuilder.Entity<Transport>().ToTable("Transport");
        modelBuilder.Entity<Event>().ToTable("Event");
    }
}