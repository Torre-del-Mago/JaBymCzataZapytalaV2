// OfetaZapłacona (OfertaID)0
using Models.Offer.DTO;

namespace Models.Offer
{
    public class PaidOfferSyncEvent : EventModel
    {
        public int OfferId { get; set; }

        public OfferSyncDTO OfferSync { get; set; }

        public List<OfferRoomSyncDTO> RoomSyncs { get; set;}
    }
}
