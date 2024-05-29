// Rezerwuj (OfertaDTO)
using Models.Offer.DTO;

namespace Models.Offer
{
    public class ReserveOfferSyncEvent : EventModel
    {
        public int Registration { get; set; }

        public OfferDTO Offer { get; set; }
    }
}
