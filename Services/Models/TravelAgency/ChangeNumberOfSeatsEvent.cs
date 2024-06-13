namespace Models.TravelAgency
{
    public class ChangeNumberOfSeatsEvent : EventModel
    {
        public int TransportId { get; set; }
        public int NumberOfSeats { get; set; }
    }
}