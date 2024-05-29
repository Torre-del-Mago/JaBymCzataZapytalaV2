
namespace Models.Transport
{
    public class CancelReservationTransportSyncEvent :EventModel
    {
        public int OfferId { get; set; }

        public int ArrivalTicketId { get; set; }
        public int ReturnTicketId { get; set; }
    }
}
