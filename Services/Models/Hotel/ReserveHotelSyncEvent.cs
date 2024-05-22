//zarezerwuj(RezerwacjaHoteluDTO)
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class ReserveHotelSyncEvent : EventModel
    {
        public HotelReservationDTO Reservation { get; set; }
    }
}