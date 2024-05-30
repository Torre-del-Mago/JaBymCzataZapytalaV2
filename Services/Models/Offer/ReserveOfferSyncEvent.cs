// Rezerwuj (OfertaDTO)
using Models.Offer.DTO;

namespace Models.Offer
{
    public class ReserveOfferSyncEvent : EventModel
    {
        public OfferDTO Offer { get; set; }


        public OfferSyncDTO OfferSync { get; set; }

        public List<OfferRoomSyncDTO> RoomSyncs { get; set; }
    }
}
