// odwołajRezerwację(offerId) bez replay
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class CancelReservationHotelSyncEvent : EventModel
    {
        public int OfferId { get; set; }
    }
}