using Microsoft.EntityFrameworkCore;
using TransportCommand.Database;
using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.FlightConnectionRepository;

public class FlightConnectionRepository : IFlightConnectionRepository
{
    private readonly TransportContext _context;

    public FlightConnectionRepository(TransportContext context)
    {
        _context = context;
    }

    public async Task<List<FlightConnection>> GetAllFlightConnectionsAsync()
    {
        return await _context.FlightConnections.ToListAsync();
    }

    public async Task<FlightConnection> GetFlightConnectionByIdAsync(int flightConnectionId)
    {
        return await _context.FlightConnections.FindAsync(flightConnectionId);
    }
}