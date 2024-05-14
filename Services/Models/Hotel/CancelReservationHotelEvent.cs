// odwołajRezerwację(offerId) bez replay
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class CancelReservationHotelEvent : EventModel
    {
        public int OfferId { get; set; }
    }
}