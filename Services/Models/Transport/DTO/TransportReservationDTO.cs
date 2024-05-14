namespace Models.Transport.DTO
{
    public class TransportReservationDTO
    {
        public int ArrivalTransportId { get; set; }

        public int ReturnTransportId { get; set; }

        public int NumberOfPeople { get; set; }

        public int OfferId { get; set; }

    }
}
