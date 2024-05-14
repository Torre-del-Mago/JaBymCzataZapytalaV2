//zarezerwuj(RezerwacjaHoteluDTO)
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class ReserveHotelEvent : EventModel
    {
        public HotelReservationDTO Reservation { get; set; }
    }
}