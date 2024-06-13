namespace Models.TravelAgency
{
    public class ChangeHotelDiscountSyncEvent : EventModel
    {
        public int HotelId { get; set; }
        public double DiscountChange { get; set; }
    }
}