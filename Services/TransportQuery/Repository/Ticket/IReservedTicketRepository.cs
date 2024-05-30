using MongoDB.Driver;
using TransportQuery.Database.Entity;

namespace TransportQuery.Repository.Ticket
{
    public interface IReservedTicketRepository
    {
        public Task<bool> ReserveTicketsAsync(int ArrivalTransportId, int ReturnTransportId, int NumberOfPeople, int OfferId,
            int ArrivalTicketId, int ReturnTicketId);

        public Task CancelTickets(int OfferId);
    }
}
