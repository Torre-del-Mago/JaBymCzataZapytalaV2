using Microsoft.EntityFrameworkCore;
using TransportCommand.Database;
using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.TransportRepository;

public class TransportRepository : ITransportRepository
{
    private readonly TransportContext _context;

    public TransportRepository(TransportContext context)
    {
        _context = context;
    }
    
    public async Task<List<Transport>> GetAllTransportsAsync()
    {
        return await _context.Transports.ToListAsync();
    }

    public async Task<Transport> GetTransportByIdAsync(int transpotId)
    {
        return await _context.Transports.FindAsync(transpotId);
    }
}