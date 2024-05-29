// Usuń Ofertę(ofertaId)
namespace Models.Offer
{
    public class RemoveOfferEvent : EventModel
    {
        public int OfferId { get; set; }
    }
}
