// oferta zarezerwowana
namespace Models.Offer
{
    public class ReservedOfferSyncEvent : EventModel
    {
        public int OfferId { get; set; }

        public enum State
        {
            RESERVED,
            NOT_RESERVED
        };

        public State Answer { get; set; }
    }
}
