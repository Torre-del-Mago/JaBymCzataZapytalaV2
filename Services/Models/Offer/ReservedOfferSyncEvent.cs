// oferta zarezerwowana
using Models.Offer.DTO;

namespace Models.Offer
{
    public class ReservedOfferSyncEvent : EventModel
    {
        public enum State
        {
            RESERVED,
            NOT_RESERVED
        };

        public State Answer { get; set; }


        public OfferSyncDTO OfferSync { get; set; }

        public List<OfferRoomSyncDTO> RoomSyncs { get; set; }
    }
}
