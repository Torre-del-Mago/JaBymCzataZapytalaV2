//  zarezerwowano ofertę (zgłoszenie id, ofertaid)
//  rezerwacja się nie powiodła (zgłoszenie id, powód)
namespace Models.Offer
{
    public class ReserveOfferEventReply : EventModel
    {
        public enum State
        {
            RESERVED,
            NOT_RESERVED
        };

        public State Answer { get; set; }

        public int OfferId { get; set; }

        public string Error { get; set; }

        public int Registration {  get; set; }
    }
}
