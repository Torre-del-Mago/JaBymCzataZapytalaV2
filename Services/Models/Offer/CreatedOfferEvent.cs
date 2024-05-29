// Stworzono Ofertę(ofertaid)
using Models.Offer.DTO;

namespace Models.Offer
{
    public class CreatedOfferEvent : EventModel
    {
        public int OfferId { get; set; }

        public OfferDTO Offer { get; set; }
    }
}
