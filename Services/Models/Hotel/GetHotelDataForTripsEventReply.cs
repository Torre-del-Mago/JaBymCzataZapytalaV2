// przyjmij dane o hotelach (Hotele_data)
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class GetHotelDataForTripsEventReply : EventModel
    {
        public HotelsDTO Hotels { get; set; }
    }
}