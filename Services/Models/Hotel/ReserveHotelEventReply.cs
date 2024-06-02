// ZarezerwowanoPokoje(ofertaID)
// BrakDostępnościPokoi(ofertaId)
namespace Models.Hotel
{
    public class ReserveHotelEventReply : EventModel
    {
        public enum State
        {
            NOT_RESERVED,
            RESERVED
        };

        public State Answer { get; set; }

        public int OfferId { get; set; }

    }
}