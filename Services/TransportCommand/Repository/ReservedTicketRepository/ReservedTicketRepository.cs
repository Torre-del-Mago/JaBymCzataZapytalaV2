using Microsoft.EntityFrameworkCore;
using TransportCommand.Database;
using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.ReservedTicketRepository;

public class ReservedTicketRepository : IReservedTicketRepository
{
    private readonly TransportContext _context;

    public ReservedTicketRepository(TransportContext context)
    {
        _context = context;
    }
    
    public async Task<List<ReservedTicket>> GetAllReservedTicketsAsync()
    {
        return await _context.ReservedTickets.ToListAsync();
    }

    public async Task<ReservedTicket> GetReservedTicketByIdAsync(int ticketId)
    {
        return await _context.ReservedTickets.FindAsync(ticketId);
    }

    public Task<List<ReservedTicket>> GetReservedTicketsByTransportId(int transportId)
    {
        return Task.FromResult(_context.ReservedTickets.Where(r => r.TransportId == transportId).ToList());
    }

    public async Task<int> insertTicket(ReservedTicket ticket)
    {
        _context.ReservedTickets.Add(ticket);
        return ticket.Id;
    }
}