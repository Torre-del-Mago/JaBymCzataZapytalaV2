// Usuń Ofertę(ofertaId)
namespace Models.Offer
{
    public class RemoveOfferSyncEvent : EventModel
    {
        public int OfferId { get; set; }
    }
}
