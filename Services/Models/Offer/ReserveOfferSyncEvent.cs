// Rezerwuj (OfertaDTO)
using Models.Offer.DTO;

namespace Models.Offer
{
    public class ReserveOfferSyncEvent : EventModel
    {

        public OfferSyncDTO OfferSync { get; set; }

        public List<OfferRoomSyncDTO> RoomSyncs { get; set; }
    }
}
