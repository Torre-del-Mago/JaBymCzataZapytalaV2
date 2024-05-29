// Rezerwuj (OfertaDTO)
using Models.Offer.DTO;

namespace Models.Offer
{
    public class ReserveOfferEvent : EventModel
    {

        public OfferDTO Offer { get; set; }
    }
}
