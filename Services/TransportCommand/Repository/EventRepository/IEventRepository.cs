using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.EventRepository
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllTicketEventsForTransportId(int transportId);

        Task InsertReservationEvent(ReservedTicket ticket);

        Task InsertCancellationEventForTickets(List<ReservedTicket> tickets);
    }
}
