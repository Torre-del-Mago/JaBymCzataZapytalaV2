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

    public Transport GetTransportById(int transpotId)
    {
        return _context.Transports.FirstOrDefault(t => t.Id == transpotId);
    }

    public async Task<Transport> GetTransportByIdAsync(int transpotId)
    {
        return await _context.Transports.FindAsync(transpotId);
    }

    public void UpdateNumberOfSeats(int transportId, int numberOfSeats)
    {
        var transport = _context.Transports.Find(transportId);
        transport.NumberOfSeats += numberOfSeats;
        _context.Transports.Update(transport);
    }

    public void UpdatePricePerSeat(int transportId, double priceChange)
    {
        var transport = _context.Transports.Find(transportId);
        transport.PricePerSeat += Convert.ToDecimal(priceChange);
        _context.Transports.Update(transport);
    }
}