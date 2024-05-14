// przyjmijDaneoHotelach(Hotele_Data)
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class GetHotelDataForTripEventReply : EventModel
    {
        public HotelDTO Hotel { get; set; }

    }
}