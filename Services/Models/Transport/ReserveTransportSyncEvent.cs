// Zarezerwuj(RezerwacjaTransportuDTO)
using Models.Transport.DTO;

namespace Models.Transport
{
    public class ReserveTransportSyncEvent : EventModel
    {
        public TransportReservationDTO Reservation { get; set; }

        public int ArrivalTicketId { get; set; }

        public int ReturnTicketId { get; set; }
    }
}