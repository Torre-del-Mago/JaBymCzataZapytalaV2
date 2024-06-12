namespace Models.Hotel
{
    public class GetHotelStatisticsEvent : EventModel
    {
        public int HotelId { get; set; }
    }
}