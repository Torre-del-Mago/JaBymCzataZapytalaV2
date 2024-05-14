// Usuń Ofertę(ofertaId)
namespace Models.Offer
{
    public class RemoveOffer : EventModel
    {
        public int OfferId { get; set; }
    }
}
