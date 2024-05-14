// Zarezerwuj(RezerwacjaTransportuDTO)
using Models.Transport.DTO;

namespace Models.Transport
{
    public class ReserveTransportEvent : EventModel
    {
        public TransportReservationDTO Reservation { get; set; }
    }
}