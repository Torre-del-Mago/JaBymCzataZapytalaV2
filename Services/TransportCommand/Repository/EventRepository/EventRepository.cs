using TransportCommand.Database;
using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly TransportContext _context;

        public EventRepository(TransportContext context)
        {
            _context = context;
        }
        public Task<List<Event>> getAllTicketEventsForTransportId(int transportId)
        {
            return Task.FromResult(_context.Events.Where(e => e.TransportId == transportId).OrderBy(e => e.Id).ToList());
        }

        public async Task insertCancellationEvent(int transportId, int ticketId)
        {
            var reservation = new Event()
            {
                EventType = EventType.Deleted,
                TransportId = transportId,
                TicketId = ticketId,
                Timestamp = DateTime.UtcNow
            };
            _context.Events.Add(reservation);
        }

        public async Task insertReservationEvent(int transportId, int ticketId)
        {
            var reservation = new Event()
            {
                EventType = EventType.Created,
                TransportId = transportId,
                TicketId = ticketId,
                Timestamp = DateTime.UtcNow
            };
            _context.Events.Add(reservation);
        }
    }
}
