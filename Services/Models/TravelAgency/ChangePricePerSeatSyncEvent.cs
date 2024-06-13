namespace Models.TravelAgency
{
    public class ChangePricePerSeatSyncEvent : EventModel
    {
        public int TransportId { get; set; }
        public double PriceChange { get; set; }
    }
}