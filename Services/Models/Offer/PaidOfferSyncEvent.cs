﻿// OfetaZapłacona (OfertaID)0
using Models.Offer.DTO;

namespace Models.Offer
{
    public class PaidOfferSyncEvent : EventModel
    {

        public OfferSyncDTO OfferSync { get; set; }

        public List<OfferRoomSyncDTO> RoomSyncs { get; set;}
    }
}
