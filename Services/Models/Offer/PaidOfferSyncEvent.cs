// OfetaZapłacona (OfertaID)0
namespace Models.Offer
{
    public class PaidOfferSyncEvent : EventModel
    {
        public int OfferId { get; set; }
    }
}
