using Models.Offer.DTO;

namespace Models.Gate.Offer.Request
{
    public class ReserveOfferRequest
    {
        public int Registration { get; set; }

        public OfferDTO Offer { get; set; }

    }
}
