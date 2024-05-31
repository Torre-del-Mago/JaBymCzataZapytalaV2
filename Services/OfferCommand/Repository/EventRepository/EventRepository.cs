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
        public void InsertCreatedEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.Created);
        }

        public void InsertReservedEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.Reserved);
        }
        public void InsertNotReservedEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.NotReserved);   
        }

        public void InsertPaidEvent(int offerId)
        {
            insertEvent(offerId, EventTypes.Paid);
        }

        public void InsertRemovedEvent(int offerId)
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
            _context.SaveChanges();
        }
    }
}
