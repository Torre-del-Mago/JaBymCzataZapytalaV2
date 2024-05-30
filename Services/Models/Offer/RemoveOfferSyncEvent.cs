// Usuń Ofertę(ofertaId)
using Models.Offer.DTO;

namespace Models.Offer
{
    public class RemoveOfferSyncEvent : EventModel
    {

        public OfferSyncDTO OfferSync { get; set; }

        public List<OfferRoomSyncDTO> RoomSyncs { get; set; }
    }
}
