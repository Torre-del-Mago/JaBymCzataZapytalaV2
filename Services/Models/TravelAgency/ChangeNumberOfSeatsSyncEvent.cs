namespace Models.TravelAgency
{
    public class ChangeNumberOfSeatsSyncEvent : EventModel
    {
        public int TransportId { get; set; }
        public int NumberOfSeats { get; set; }
    }
}