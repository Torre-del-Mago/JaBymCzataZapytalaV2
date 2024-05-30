using Models.Offer.DTO;

namespace OfferCommand.Repository.EventRepository
{
    public interface IEventRepository
    {
        public void insertCreatedEvent(int offerId);

        public void insertReservedEvent(int offerId);
        public void insertNotReservedEvent(int offerId);
        public void insertRemovedEvent(int offerId);
        public void insertPaidEvent(int offerId);
        public void insertNotPaidInTimeEvent(int offerId);
    }
}
