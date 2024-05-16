using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.ReservedTicketRepository;

public interface IReservedTicketRepository
{
    public Task<List<ReservedTicket>> GetAllReservedTicketsAsync();

    public Task<ReservedTicket> GetReservedTicketByIdAsync(int ticketId);
}