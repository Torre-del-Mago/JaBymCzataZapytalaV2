namespace Models.TravelAgency
{
    public class ChangeHotelDiscountEvent : EventModel
    {
        public int HotelId { get; set; }
        public double DiscountChange { get; set; }
    }
}