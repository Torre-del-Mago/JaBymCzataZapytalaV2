// oferta zarezerwowana
namespace Models.Offer
{
    public class ReservedOfferSyncEvent : EventModel
    {
        public int OfferId { get; set; }
    }
}
