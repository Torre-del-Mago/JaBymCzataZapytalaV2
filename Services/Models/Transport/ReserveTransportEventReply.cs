// zarezerwowanolot(ofertaid)
// brak biletów (ofertaid)
namespace Models.Transport
{
    public class ReserveTransportEventReply : EventModel
    {
        public enum State
        {
            RESERVED,
            NOT_RESERVED
        };

        public State Answer { get; set; }

        public int OfferId { get; set; }

    }
}