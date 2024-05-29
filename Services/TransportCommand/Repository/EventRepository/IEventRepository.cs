using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.EventRepository
{
    public interface IEventRepository
    {
        public Task<List<Event>> getAllTicketEventsForTransportId(int transportId);

        public Task insertReservationEvent(ReservedTicket ticket);

        public Task insertCancellationEventForTickets(List<ReservedTicket> tickets);
    }
}
