namespace Models.Hotel
{
    public class GetHotelStatisticsEventReply : EventModel
    {
        public bool IsSomeoneElseWatching { get; set; }
        public bool HasSomebodyReservedHotel { get; set; }
    }
}