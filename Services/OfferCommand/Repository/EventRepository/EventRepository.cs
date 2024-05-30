using OfferCommand.Database;

namespace OfferCommand.Repository.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private OfferContext _context;
        public EventRepository(OfferContext context)
        {
            _context = context;
        }
        public void insertCreatedEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.Created);
        }

        public void insertReservedEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.Reserved);
        }
        public void insertNotReservedEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.NotReserved);   
        }

        public void insertPaidEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.Paid);
        }

        public void insertRemovedEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.Removed);
        }

        private void insertEvent(int offerId, string eventType) {

            _context.Events.Add(new Database.Tables.OfferEvent()
            {
                OfferId = offerId,
                EventType = eventType,
                TimeStamp = DateTime.Now
            });
        }
    }
}
