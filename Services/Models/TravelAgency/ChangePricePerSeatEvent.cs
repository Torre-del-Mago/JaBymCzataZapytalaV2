namespace Models.TravelAgency
{
    public class ChangePricePerSeatEvent : EventModel
    {
        public int TransportId { get; set; }
        public double PriceChange { get; set; }
    }
}