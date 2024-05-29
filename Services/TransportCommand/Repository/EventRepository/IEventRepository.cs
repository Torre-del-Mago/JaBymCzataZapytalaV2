using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.EventRepository
{
    public interface IEventRepository
    {
        public Task<List<Event>> getAllTicketEventsForTransportId(int transportId);

        public Task insertReservationEvent(int transportId, int ticketId);

        public Task insertCancellationEvent(int transportId, int ticketId);
    }
}
