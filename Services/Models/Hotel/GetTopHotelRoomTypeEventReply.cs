using Models.Admin.DTO;

namespace Models.Hotel
{
    public class GetTopHotelRoomTypeEventReply : EventModel
    {
        public TopHotelsDTO TopHotelsDto { get; set; }
        public TopRoomTypesDTO TopRoomTypesDto { get; set; }
    }
}