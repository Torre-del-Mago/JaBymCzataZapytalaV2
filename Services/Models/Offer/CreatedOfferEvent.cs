// Stworzono Ofertę(ofertaid)
namespace Models.Offer
{
    public class CreatedOfferEvent : EventModel
    {
        public int OfferId { get; set; }
    }
}
