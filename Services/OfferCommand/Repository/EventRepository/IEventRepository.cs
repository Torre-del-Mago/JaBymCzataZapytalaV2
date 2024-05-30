using Models.Offer.DTO;

namespace OfferCommand.Repository.EventRepository
{
    public interface IEventRepository
    {
        void InsertCreatedEvent(int offerId);
        void InsertReservedEvent(int offerId);
        void InsertNotReservedEvent(int offerId);
        void InsertRemovedEvent(int offerId);
        void InsertPaidEvent(int offerId);
    }
}
